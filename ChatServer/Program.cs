using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            RunServer().GetAwaiter().GetResult();
        }

        private static async Task RunServer()
        {
            await Server.StartServerAsync();
            await Task.Delay(-1);
        }
    }
}
