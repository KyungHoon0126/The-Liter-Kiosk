using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using THE_LITER_KIOSK.Network.Model;

namespace THE_LITER_KIOSK.Network
{
    public class TcpClient
    {
        private const string ip = "10.80.162.152";
        private const int port = 80;

        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);
            
        private static string response = string.Empty;


        public void StartClient(string id)
        {
#if true
            var json = new JObject();
            var obj = new JArray();

            json.Add("MSGType", 0);
            json.Add("Id", id);
            json.Add("ShopName", "");
            json.Add("Content", "?");
            json.Add("OrderNumber", "");
            json.Add("Menus", obj);
#endif

            try {

                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), client);
                connectDone.WaitOne();

                Debug.WriteLine(client.Connected);
                
                Send(client, json.ToString());
                sendDone.WaitOne();

                Receive(client);
                receiveDone.WaitOne();
            }
            catch (Exception e)
            {
                Debug.WriteLine("START CLIENT ERROR : " + e.Message);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                
                Debug.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
                
                connectDone.Set();
            }
            catch (Exception e)
            {
                Debug.WriteLine("CONNECT CALL BACK ERROR : " +  e.Message);
            }
        }

        private void Receive(Socket client)
        {
            try
            {
                StateObjectModel state = new StateObjectModel();
                state.workSocket = client;
                client.BeginReceive(state.buffer, 0, StateObjectModel.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Debug.WriteLine("RECEIVE ERROR : " + e.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObjectModel state = (StateObjectModel)ar.AsyncState;
                Socket client = state.workSocket;

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    client.BeginReceive(state.buffer, 0, StateObjectModel.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                        Debug.WriteLine(response);
                    }
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("RECEIVE CALL BACK ERROR : " + e.Message);
            }
        }

        private void Send(Socket client, string data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);
            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                int bytesSent = client.EndSend(ar);
                
                Debug.WriteLine("Sent{0} bytes to server.", bytesSent);
                
                sendDone.Set();
            }
            catch (Exception e)
            {
                Debug.WriteLine("SEND CALL BACK ERROR : " + e.Message);
            }
        }

        public bool CheckServerState()
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
            client.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), client);
            return client.Connected ? true : false;
        }
    }
}
