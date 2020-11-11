using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace THE_LITER_KIOSK.Service
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

        public class AsynchronouseClient
        {
            // The port number fro the remote device
            private const int port = 80;
            private const string ip = "10.80.162.152";

            // ManualResetEvent instances signal completion.
            private static ManualResetEvent connectDone =
                new ManualResetEvent(false);
            private static ManualResetEvent sendDone =
                new ManualResetEvent(false);
            private static ManualResetEvent receiveDone =
                new ManualResetEvent(false);

            // The response from the remote device
            private static String response = String.Empty;

            private static void StartClient()
            {
                var json = new JObject();
                var array = new JObject();
                var obj = new JArray();
                /* array.Add("Name", "");
                array.Add("Count", "");
                array.Add("Price", ""); */
                // obj.Add();
                // List
                /* for (int i = 0; i < List.lenght; i++)
                {
                    JObject jObject = new JObject();
                    jObject["idx"] = List[i].idx;
                    array.Add(jObject);
                }*/

                json.Add("MSGType", 1);
                json.Add("Id", 2106);
                json.Add("ShopName", "");
                json.Add("Content", "?");
                json.Add("OrderNumber", "");
                json.Add("Menus", obj);

                try
                {
                    //IPHostEntry iPHostEntry = Dns.GetHostEntry(ip);
                    //Debug.WriteLine(iPHostEntry);
                    //IPAddress ipAdress = iPHostEntry.AddressList[0];
                    //IPEndPoint remoteEp = new IPEndPoint(ipAdress, port);

                    Socket client = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream, ProtocolType.Tcp);

                    //client.BeginConnect(remoteEp,
                    //new AsyncCallback(ConnectCallback), client);

                    client.BeginConnect("10.80.162.152", 80,
                    new AsyncCallback(ConnectCallback), client);
                    connectDone.WaitOne();

                    Send(client, json.ToString());
                    Console.WriteLine(json.ToString());
                    sendDone.WaitOne();

                    Receive(client);
                    receiveDone.WaitOne();

                    Console.WriteLine("RES receive : {0}", response);

                    //Release the socket
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.WriteLine(json.ToString());
                    Debug.WriteLine(e.Message);
                }
            }

            private static void ConnectCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the socket from the state obj
                    Socket client = (Socket)ar.AsyncState;

                    // Complete the connection.
                    client.EndConnect(ar);

                    Console.WriteLine("Socket connected to {0}",
                        client.RemoteEndPoint.ToString());

                    // Signal that the connection has been made.
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
                    // Create the state obj
                    StateObject state = new StateObject();
                    state.workSocket = client;

                    // Begin receving the data from the remote device.
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
                    // Retrieve the state obj and the client socket
                    // from the asynchonouse state obj.
                    StateObject state = (StateObject)ar.AsyncState;
                    Socket client = state.workSocket;

                    // Read data from the remote device.
                    int bytesRead = client.EndReceive(ar);

                    if (bytesRead > 0)
                    {
                        // There might be more data, so store the data recieved so far.
                        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                        // Get the rest of the data
                        client.BeginReceive(state.buffer,
                            0,
                            StateObject.BufferSize,
                            0,
                            new AsyncCallback(ReceiveCallback),
                            state);
                    }
                    else
                    {
                        // All the data has arrived put it in res
                        if (state.sb.Length > 1)
                        {
                            response = state.sb.ToString();
                            Console.WriteLine(response);
                        }
                        // Signal that all bytes have been received.
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
                // Convert the string data to byte data using ASCII encoding.
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin  sending the data to remote device.
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
                    // Retrieve the socket from the state obj.
                    Socket client = (Socket)ar.AsyncState;

                    //Complete sending the data to the remote device.
                    int bytesSent = client.EndSend(ar);
                    Console.WriteLine("Sent{0} bytes to server.", bytesSent);

                    //Signal that all bytes have benn sent.
                    sendDone.Set();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            public static int Main(string[] args)
            {
                StartClient();
                return 0;
            }
        }

    }
}
