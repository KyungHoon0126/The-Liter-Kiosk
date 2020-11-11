using MySql.Data.MySqlClient.Memcached;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;

namespace THE_LITER_KIOSK.Network
{
    public class TcpClient
    {
        public class StateObject
        {
            public Socket workSocket = null;
            public const int BufferSize = 4600;
            public byte[] buffer = new byte[BufferSize];
            public StringBuilder sb = new StringBuilder();
        }

        
            private const int port = 80;
            private const string ip = "100.80.162.152";
        
            private static ManualResetEvent connectDone =
                new ManualResetEvent(false);
            private static ManualResetEvent sendDone =
                new ManualResetEvent(false);
            private static ManualResetEvent receiveDone =
                new ManualResetEvent(false);
            
            private static String response = String.Empty;

            public void StartClient(string id)
            {

                var json = new JObject();
                var obj = new JArray();

                json.Add("MSGType", 0);
                json.Add("Id", id);
                json.Add("ShopName", "");
                json.Add("Content", "?");
                json.Add("OrderNumber", "");
                json.Add("Menus", obj);

                try {

                    Socket client = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);
                    
                    client.BeginConnect(ip, port,
                    new AsyncCallback(ConnectCallback), client);
                    connectDone.WaitOne();
                    Debug.WriteLine(client.Connected);
                    Send(client, json.ToString());
                
                    sendDone.WaitOne();

                    Receive(client);
                    receiveDone.WaitOne();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Debug.WriteLine(e.Message);
                }
            }

            private static void ConnectCallback(IAsyncResult ar)
            {
                try
                {
                    Socket client = (Socket)ar.AsyncState;

                    client.EndConnect(ar);

                    Console.WriteLine("Socket connected to {0}",
                        client.RemoteEndPoint.ToString());

                    connectDone.Set();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            private static void Receive(Socket client)
            {
                try
                {
                    StateObject state = new StateObject();
                    state.workSocket = client;

                    client.BeginReceive(state.buffer,
                        0,
                        StateObject.BufferSize,
                        0,
                        new AsyncCallback(ReceiveCallback),
                        state);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            private static void ReceiveCallback(IAsyncResult ar)
            {
                try
                {
                    StateObject state = (StateObject)ar.AsyncState;
                    Socket client = state.workSocket;

                    int bytesRead = client.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                        client.BeginReceive(state.buffer,
                            0,
                            StateObject.BufferSize,
                            0,
                            new AsyncCallback(ReceiveCallback),
                            state);
                    }
                    else
                    {
                        if (state.sb.Length > 1)
                        {
                            response = state.sb.ToString();
                            Console.WriteLine(response);
                        }
                        receiveDone.Set();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            private static void Send(Socket client, String data)
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                client.BeginSend(byteData,
                    0,
                    byteData.Length,
                    0,
                    new AsyncCallback(SendCallback),
                    client);
            }

            private static void SendCallback(IAsyncResult ar)
            {
                try
                {
                    Socket client = (Socket)ar.AsyncState;

                    int bytesSent = client.EndSend(ar);
                    Console.WriteLine("Sent{0} bytes to server.", bytesSent);

                    sendDone.Set();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        public bool CheckServerState()
        {
            Socket client = new Socket(AddressFamily.InterNetwork,
                       SocketType.Stream, ProtocolType.Tcp);
            client.BeginConnect(ip, port,
            new AsyncCallback(ConnectCallback), client);
            Debug.WriteLine(client.Connected);
            
            if(client.Connected)
            {
                return true;
            } else
            {
                return false;
            }
           
        }
        

    }
}
