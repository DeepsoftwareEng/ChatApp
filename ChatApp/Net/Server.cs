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
        public PacketReader packetReader;
        public event Action connectedEvent;
        public event Action msgReceiveEvent;
        public event Action disconnectEvent;
        public Server()
        {
            client = new TcpClient();
        }

        public void ConnectToServer(string username)
        {
            if (!client.Connected)
            {
                client.Connect("192.168.1.3", 49398);
                packetReader = new PacketReader(client.GetStream());
                if (!string.IsNullOrEmpty(username))
                {
                    var connectpacket = new PacketBuilder();
                    connectpacket.WriteOpCode(0);
                    connectpacket.WriteMessage(username);
                    client.Client.Send(connectpacket.GetPacketByte());
                }
                ReadPackets();
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    var opcode = packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            msgReceiveEvent?.Invoke();
                            break;
                        case 10:
                            disconnectEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("ah yes..");
                            break;
                    }
                }
            });
        }
        public void SendMessageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            client.Client.Send(messagePacket.GetPacketByte());
        }
    }
}
