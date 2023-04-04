using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace GameServer
{
    class Client
    {

        public static int dataBufferSize = 4096;

        private int ID;
        public TCP tcp;
       
        public Client(int clientId)
        {
            ID = clientId;
            tcp = new TCP(ID);
        }

        public class TCP
        {
            public TcpClient Socket;
            private readonly int _id;
            private NetworkStream stream;
            private byte[] reciveBuffer;
            public TCP(int id)
            {
                _id = id;
            }

            public void Connect(TcpClient socket)
            {
                Socket = socket;
                Socket.ReceiveBufferSize = dataBufferSize;
                Socket.SendBufferSize = dataBufferSize;

                stream = Socket.GetStream();

                reciveBuffer = new byte[dataBufferSize];

                stream.BeginRead(reciveBuffer, 0, dataBufferSize,ReciveCallback, null);
            }

            private void ReciveCallback(IAsyncResult asyncResult)
            {
                try
                {
                    int byteLen = stream.EndRead(asyncResult);
                    if(byteLen <= 0)
                    {
                        // disconnect
                        return;
                    }

                    byte[] _data = new byte[byteLen];
                    Array.Copy(reciveBuffer, _data, byteLen);

                    // Handle Data's

                    stream.BeginRead(reciveBuffer, 0, dataBufferSize, ReciveCallback, null);

                }
                catch (Exception exp)
                {
                    Console.WriteLine($"Error receiving TCP data : {exp} ");
                    // disconnect
                }
            }
        }
    }
    
}
