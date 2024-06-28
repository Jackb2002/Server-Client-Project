using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ChatClient
{
    internal class Client
    {
        private static RSAParameters _privateKey;
        private static RSAParameters _publicKey;
        private static RSAParameters _serverPublicKey;

        static Client()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                _privateKey = rsa.ExportParameters(true);
                _publicKey = rsa.ExportParameters(false);
            }
            Debug.WriteLine("Client, RSA keys generated");
        }

        public static void SetServerPublicKey(RSAParameters publicKey)
        {
            _serverPublicKey = publicKey;
        }

        public static string Encrypt(string plainText)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(_serverPublicKey);
                var data = Encoding.UTF8.GetBytes(plainText);
                var encryptedData = rsa.Encrypt(data, false);
                Debug.WriteLine("Client, Message encrypted");
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
                Debug.WriteLine("Client, Message decrypted");
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        public static async Task ConnectAndCommunicateAsync(string serverIp, int serverPort)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    await client.ConnectAsync(serverIp, serverPort);
                    Debug.WriteLine("Client, Connected to server");

                    using (var stream = client.GetStream())
                    {
                        var buffer = new byte[1024];

                        // Receive server's public key
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            Debug.WriteLine("Client, Server disconnected before sending public key");
                            return;
                        }

                        var serverPublicKeyString = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        var serverPublicKeyParts = serverPublicKeyString.Split(',');

                        _serverPublicKey = new RSAParameters
                        {
                            Modulus = Convert.FromBase64String(serverPublicKeyParts[0]),
                            Exponent = Convert.FromBase64String(serverPublicKeyParts[1])
                        };

                        Debug.WriteLine("Client, Received public key from server");

                        // Send client's public key
                        var clientPublicKeyString = Convert.ToBase64String(_publicKey.Modulus) + "," + Convert.ToBase64String(_publicKey.Exponent);
                        var clientPublicKeyData = Encoding.UTF8.GetBytes(clientPublicKeyString);
                        await stream.WriteAsync(clientPublicKeyData, 0, clientPublicKeyData.Length);
                        Debug.WriteLine("Client, Sent public key to server");

                        while (true)
                        {
                            Console.Write("Enter message: ");
                            var message = Console.ReadLine();
                            if (string.IsNullOrEmpty(message))
                            {
                                Debug.WriteLine("Client, No message entered, ending communication");
                                break;
                            }

                            var encryptedMessage = Encrypt(message);
                            var messageData = Encoding.UTF8.GetBytes(encryptedMessage);

                            await stream.WriteAsync(messageData, 0, messageData.Length);
                            Debug.WriteLine("Client, Encrypted message sent to server");

                            bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                            if (bytesRead == 0)
                            {
                                Debug.WriteLine("Client, Server disconnected during read");
                                break;
                            }

                            var encryptedResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            Debug.WriteLine("Client, Encrypted response received");
                            var decryptedResponse = Decrypt(encryptedResponse);

                            Console.WriteLine($"Server response: {decryptedResponse}");
                            Debug.WriteLine($"Client, Decrypted response: {decryptedResponse}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Client, Error: {ex.Message}");
            }
        }
    }
}
