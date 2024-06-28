using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text.Json;

namespace ChatServer
{
    internal class Message
    {
        public string Content { get; set; }
        public string Sender { get; set; }
        public string Type { get; set; }
        public DateTime Time { get; set; }

        public Message(string content, string sender, string type)
        {
            Content = content;
            Sender = sender;
            Type = type;
            Time = DateTime.Now;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Message FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Message>(json);
            }
            catch (Newtonsoft.Json.JsonException ex)
            {
                Debug.WriteLine($"Failed to deserialize JSON: {ex.Message}");
                return null;
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case "ClientText":
                    return $"{Time:HH:mm:ss} {Sender}: {Content}";
                case "ServerText":
                    return $"{Time:HH:mm:ss} Broadcast: {Content}";
                case "Image":
                    return $"{Time:HH:mm:ss} {Sender} sent an image";
                case "File":
                    return $"{Time:HH:mm:ss} {Sender} sent a file";
                case "ServerCommand":
                    return $"{Time:HH:mm:ss} Server command: {Content}";
                case "ClientCommand":
                    return $"{Time:HH:mm:ss} {Sender} command: {Content}";
                case "ServerHeartbeat":
                    return $"{Time:HH:mm:ss} Server heartbeat: {Content}";
                case "Aknowledgement":
                    return $"{Time:HH:mm:ss} {Sender} aknowledged: {Content}";
                default:
                    Debug.WriteLine($"Message has invalid message type - \"{Type}\"");
                    return $"{Time:HH:mm:ss} {Sender}: {Content}";
            }
        }
    }
}
