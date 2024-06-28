using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            await Client.ConnectAndCommunicateAsync("127.0.0.1", 8000);
        }
    }
}
