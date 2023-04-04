using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace GameServer
{
    class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        private static TcpListener tcp;
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();

        public static void Start(int maxPlayer, int port)
        {
            MaxPlayers = maxPlayer;
            Port = port;
            Console.WriteLine("Starting Server ... (please wait...)");
            InitializeServerData();

            tcp = new TcpListener(IPAddress.Any, port);
            tcp.Start();
            tcp.BeginAcceptTcpClient(new AsyncCallback(ConnectCallBack), null);

            Console.WriteLine($"Server Starting in Port = {port}");
        }
        private static void ConnectCallBack(IAsyncResult result)
        {
            TcpClient client = tcp.EndAcceptTcpClient(result);
            tcp.BeginAcceptTcpClient(new AsyncCallback(ConnectCallBack), null);

            Console.WriteLine($"Incomming Connection Client ... (from :  {client.Client.RemoteEndPoint} )");
            for (int i = 0; i < MaxPlayers; i++)
            {
                if(clients[i].tcp.Socket == null)
                {
                    clients[i].tcp.Connect(client);
                    return;
                }
            }
            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect : Server Full !");
        }

        private static void InitializeServerData()
        {
            for (int i = 0; i < MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
        }
    }
}
