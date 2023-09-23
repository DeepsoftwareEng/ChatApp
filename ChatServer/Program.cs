using ChatServer.Net.IO;
using System;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
        static List<Client> users;
        static TcpListener listener;
        static void Main(string[] args)
        {
            users = new List<Client>();
            listener = new TcpListener(IPAddress.Parse("192.168.1.3"), 49398);
            listener.Start();
            while (true)
            {
                var client = new Client(listener.AcceptTcpClient());
                users.Add(client);
                //Broadcast the coonection to everyopne on the server
                BroadcastConnection();

            } 
        }
        static void BroadcastConnection()
        {
            foreach(var user in users)
            {
                foreach(var usr in users)
                {
                    var broadcastpacket = new PacketBuilder();
                    broadcastpacket.WriteOpCode(1);
                    broadcastpacket.WriteMessage(usr.Username);
                    broadcastpacket.WriteMessage(usr.UID.ToString());
                    user.ClientSocket.Client.Send(broadcastpacket.GetPacketByte());
                }
            }
        }
        public static void BroadcastMessage(string message)
        {
            foreach(var user in users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                user.ClientSocket.Client.Send(msgPacket.GetPacketByte());
            }
        }
        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUser = users.Where(x=> x.UID.ToString() == uid).FirstOrDefault();  
            users.Remove(disconnectedUser);
            foreach(var user in users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                user.ClientSocket.Client.Send(broadcastPacket.GetPacketByte());
            }
            BroadcastMessage($"{disconnectedUser.Username} has disconnected!");
        }
    }
}