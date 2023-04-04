using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Net;

public class client : MonoBehaviour
{
    public static client instance;
    public int port = 26950, myId = 0;
    public TCP tcp;
    public static int dataBufferSize = 4096;
    public string ip = "127.0.0.1";

    private void Awake()
    {
        if(instance == null)
        instance = this;
        else if(instance != this)
        {
            print("Instance already exists, destroyed object !");
            Destroy(this);
        }       
    }

    private void Start()
    {
        tcp = new TCP();
    }

    public void ConnectedToServer() => tcp.Connect();

    public class TCP
    {
        public TcpClient socket;
        private NetworkStream stream;
        private byte[] reciveData;
       
        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };
            reciveData = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }
        private void ConnectCallback(IAsyncResult asyncResult)
        {
            socket.EndConnect(asyncResult);

            if (!socket.Connected) return;

            stream = socket.GetStream();
            stream.BeginRead(reciveData, 0, dataBufferSize, ReciveCallback, null);
        }

        private void ReciveCallback(IAsyncResult asyncResult)
        {
            try
            {
                int byteLen = stream.EndRead(asyncResult);
                if (byteLen <= 0)
                {
                    // disconnect
                    return;
                }

                byte[] _data = new byte[byteLen];
                Array.Copy(reciveData, _data, byteLen);

                // Handle Data's

                stream.BeginRead(reciveData, 0, dataBufferSize, ReciveCallback, null);

            }
            catch (Exception exp)
            {
                Console.WriteLine($"Error receiving TCP data : {exp} ");
                // disconnect
            }
        }
    }
}    
