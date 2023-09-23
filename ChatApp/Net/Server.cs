using ChatApp.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Net
{
    class Server
    {
        TcpClient client;
        public Server()
        {
            client = new TcpClient();
        }

        public void ConnectToServer(string username)
        {
            if (!client.Connected)
            {
                client.Connect("192.168.1.3", 49398);
                var connectpacket = new PacketBuilder();
                connectpacket.WriteOpCode(0);
                connectpacket.WriteString(username);
                client.Client.Send(connectpacket.GetPacketByte());
            }
        }
    }
}
