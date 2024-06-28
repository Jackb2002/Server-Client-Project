using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            ConnectClient();
        }

        internal async void ConnectClient()
        {
            await Client.ConnectAsync("127.0.0.1", 8000);
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Starting message task");
            Task.Run(SendTextboxMessage);
        }

        private async Task SendTextboxMessage()
        {
            var message = string.Empty;
            inputBox.Invoke((MethodInvoker)(() =>
            {
                message = inputBox.Text.Trim();
            }));

            if (string.IsNullOrEmpty(message))
            {
                Debug.WriteLine("Message task failed on empty message");
                return;
            }

            Debug.WriteLine("Sending message length " + message.Length + " to server");

            string localPCName = Environment.MachineName;
            Message msg = new Message(message, localPCName, "ClientText");
            string jsonMessage = msg.ToJson();

            await Client.SendMessageAsync(jsonMessage);

            inputBox.Invoke((MethodInvoker)(() =>
            {
                inputBox.Clear();
            }));
        }
    }
}
