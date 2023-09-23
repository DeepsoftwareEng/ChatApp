using ChatServer.Net.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Client
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public TcpClient ClientSocket { get; set; }
        PacketReader packetReaader;
        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            packetReaader = new PacketReader(ClientSocket.GetStream());
            var opcode = packetReaader.ReadByte();
            Username = packetReaader.ReadMessage();
            Console.WriteLine($"{DateTime.Now}: Client has connected as {Username}");
            Task.Run(() => Process());
        }
        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = packetReaader.ReadByte();
                    switch (opcode)
                    {
                        case 5:
                            var msg = packetReaader.ReadMessage();
                            Console.WriteLine($"{DateTime.Now}: Message Receive:{msg}");
                            Program.BroadcastMessage( msg );
                            break;
                        default:
                            break;
                    }
                }catch(Exception ex)
                {
                    Console.WriteLine($"[{UID.ToString()}]: Disconnected");
                    Program.BroadcastDisconnect(UID.ToString());
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
