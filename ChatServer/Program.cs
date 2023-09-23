using System;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
        static List<Client> user;
        static TcpListener listener;
        static void Main(string[] args)
        {
            user = new List<Client>();
            listener = new TcpListener(IPAddress.Parse("192.168.1.3"), 49398);
            listener.Start();
            while (true)
            {
                var client = new Client(listener.AcceptTcpClient());
                user.Add(client);
                //Broadcast the coonection to everyopne on the server


            } 
        }
    }
}