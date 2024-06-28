using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    internal class Server
    {
        private static RSAParameters _privateKey;
        private static RSAParameters _publicKey;
        private static ConcurrentDictionary<TcpClient, RSAParameters> _clientPublicKeys = new ConcurrentDictionary<TcpClient, RSAParameters>();

        static Server()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                _privateKey = rsa.ExportParameters(true);
                _publicKey = rsa.ExportParameters(false);
            }
            Debug.WriteLine("Server, RSA keys generated");
        }

        public static string Encrypt(string plainText, RSAParameters publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey);
                var data = Encoding.UTF8.GetBytes(plainText);
                var encryptedData = rsa.Encrypt(data, false);
                Debug.WriteLine("Server, Message encrypted");
                return Convert.ToBase64String(encryptedData);
            }
        }

        public static string Decrypt(string encryptedText)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(_privateKey);
                var data = Convert.FromBase64String(encryptedText);
                var decryptedData = rsa.Decrypt(data, false);
                Debug.WriteLine("Server, Message decrypted");
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        public static async Task StartServerAsync()
        {
            var listener = new TcpListener(IPAddress.Any, 8000);
            listener.Start();
            Debug.WriteLine("Server, Server started");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                Debug.WriteLine("Server, Client connected");
                _ = HandleClientAsync(client);
            }
        }

        private static async Task HandleClientAsync(TcpClient client)
        {
            var buffer = new byte[1024];
            var stream = client.GetStream();

            try
            {
                // Send server public key to client
                var serverPublicKeyString = Convert.ToBase64String(_publicKey.Modulus) + "," + Convert.ToBase64String(_publicKey.Exponent);
                var serverPublicKeyData = Encoding.UTF8.GetBytes(serverPublicKeyString);
                await stream.WriteAsync(serverPublicKeyData, 0, serverPublicKeyData.Length);
                Debug.WriteLine("Server, Sent public key to client");

                // Receive client's public key
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                var clientPublicKeyString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var clientPublicKeyParts = clientPublicKeyString.Split(',');

                var clientPublicKey = new RSAParameters
                {
                    Modulus = Convert.FromBase64String(clientPublicKeyParts[0]),
                    Exponent = Convert.FromBase64String(clientPublicKeyParts[1])
                };

                _clientPublicKeys[client] = clientPublicKey;
                Debug.WriteLine("Server, Received public key from client");

                while (true)
                {
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    var encryptedText = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Debug.WriteLine("Server, Encrypted message received");
                    var decryptedText = Decrypt(encryptedText);
                    Debug.WriteLine($"Server, Decrypted message: {decryptedText}");

                    var response = "Message received";
                    var encryptedResponse = Encrypt(response, clientPublicKey);
                    var responseData = Encoding.UTF8.GetBytes(encryptedResponse);

                    await stream.WriteAsync(responseData, 0, responseData.Length);
                    Debug.WriteLine("Server, Encrypted response sent to client");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Server, Error: {ex.Message}");
            }
            finally
            {
                _clientPublicKeys.TryRemove(client, out _);
                client.Close();
                Debug.WriteLine("Server, Client disconnected");
            }
        }
    }
}
