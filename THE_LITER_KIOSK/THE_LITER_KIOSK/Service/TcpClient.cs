using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace THE_LITER_KIOSK.Service
{
    class TcpClient
    {
        public class StateObject
        {
            //Client Socket.
            public Socket workSocket = null;
            //Size of receive buffer
            public const int BufferSize = 4600;
            //Recieve buffer
            public byte[] buffer = new byte[BufferSize];
            //Recive data string.
            public StringBuilder sb = new StringBuilder();
        }

        public class AsynchronouseClient
        {
            // The port number fro the remote device
            private const int port = 80;

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
                array.Add("Name", "아무거나");
                array.Add("Count", 10000);
                array.Add("Price", 1000000000);
                obj.Add(array);
                // List
                /* for (int i = 0; i < List.lenght; i++)
                {
                    JObject jObject = new JObject();
                    jObject["idx"] = List[i].idx;
                    array.Add(jObject);
                }*/

                json.Add("MSGType", 2);
                json.Add("Id", 2106);
                json.Add("ShopName", "asdfgjgdsdfghjklasdfghjkasdfghj");
                json.Add("Content", "content");
                json.Add("OrderNumber", "001");
                json.Add("Menus", obj);

                try
                {

                    // Estalish the remote endpoint for the socket.
                    // The name of the remote device is "host.contoso.com".
                    IPHostEntry iPHostEntry = Dns.GetHostEntry("10.80.162.152");
                    IPAddress ipAdress = iPHostEntry.AddressList[0];
                    IPEndPoint remoteEp = new IPEndPoint(ipAdress, port);

                    // Create a TCP/IP socket.
                    Socket client = new Socket(ipAdress.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

                    // Connect to the remote endpoint.
                    client.BeginConnect(remoteEp,
                    new AsyncCallback(ConnectCallback), client);
                    connectDone.WaitOne();

                    // Send test data to the remote device.
                    Send(client, json.ToString());
                    Console.WriteLine(json.ToString());
                    sendDone.WaitOne();

                    // Receive the response from the remote device
                    Receive(client);
                    receiveDone.WaitOne();

                    //Write the respones to the console.
                    Console.WriteLine("RES receive : {0}", response);

                    //Release the socket
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.WriteLine(json.ToString());
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
        }
    }
}
