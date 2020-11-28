using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TheLiter.Core.Network;
using TheLiter.Core.Network.Model;

namespace THE_LITER_KIOSK.Network
{
    public class NetworkManager
    {
        private const string ip = "10.80.162.152";
        //private const string ip = "10.80.163.141"; // 용빈 Computer
        private const int port = 80;

        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);
        private static ManualResetEvent disconnectDone =
            new ManualResetEvent(false);
            
        private static string response = string.Empty;

        public void ConnectSocket(TcpModel tcpModel)
        {
            try 
            {
                //while (true)
                //{
                    if(tcpModel != null)
                    {
                        TcpHelper.SocketClient.BeginConnect(ip, port, new AsyncCallback(ConnectCallback), TcpHelper.SocketClient);
                        connectDone.WaitOne(); // 완료되기를 기다림.

                        Debug.WriteLine(TcpHelper.SocketClient.Connected);

                        Send(TcpHelper.SocketClient, SetOrderMsgArgs(tcpModel));
                        sendDone.WaitOne();

                        Receive(TcpHelper.SocketClient);
                        receiveDone.WaitOne();

                        if (TcpHelper.SocketClient.Connected)
                        {
                            Debug.WriteLine("We're still connected");
                        }
                        else
                        {
                            Debug.WriteLine("We're disconnected");
                        }
                    }
                    tcpModel = null;
                //}
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

        public void Send(Socket client, string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
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

        public void DisconnectSocket()
        {
            if (TcpHelper.SocketClient.Connected)
            {
                TcpHelper.SocketClient.Shutdown(SocketShutdown.Both);
                TcpHelper.SocketClient.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), TcpHelper.SocketClient);
                disconnectDone.WaitOne();
            }
        }

        private void DisconnectCallback(IAsyncResult ar)
        {
            TcpHelper.SocketClient = (Socket)ar.AsyncState;
            TcpHelper.SocketClient.EndDisconnect(ar);
            disconnectDone.Set();
        }

        public bool CheckServerState()
        {
            try
            {
                Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ip, port);
                return client.Connected ? true : false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("CHECK SERVER STATE ERROR : " + e.Message);
                return false;
            }
        }

        public string SetOrderMsgArgs(TcpModel tcpModel)
        {
            var jObject = new JObject();
            var jArray = new JArray();

            for (int i = 0; i < tcpModel.MenuItems.Count; i++)
            {
                JObject menuObject = new JObject();

                menuObject["Name"] = tcpModel.MenuItems[i].Name;
                menuObject["Price"] = tcpModel.MenuItems[i].Price;
                menuObject["Count"] = tcpModel.MenuItems[i].Count;

                jArray.Add(menuObject);
            }

            jObject["MSGType"] = tcpModel.MessageType;
            jObject["id"] = tcpModel.Id;
            jObject["ShopName"] = tcpModel.ShopName;
            jObject["Content"] = tcpModel.Content;
            jObject["OrderNumber"] = tcpModel.OrderNumber;
            jObject["Menus"] = jArray;

            return JsonConvert.SerializeObject(jObject);
        }

        public string SetGroupMsgArgs(MessageModel messageModel)
        {
            var jObject = new JObject();
            var jArray = new JArray();

            for (int i = 0; i < messageModel.MenuItems.Count; i++)
            {
                JObject menuObject = new JObject();

                menuObject["Name"] = messageModel.MenuItems[i].Name;
                menuObject["Price"] = messageModel.MenuItems[i].Price;
                menuObject["Count"] = messageModel.MenuItems[i].Count;

                jArray.Add(menuObject);
            }

            jObject["MSGType"] = messageModel.MessageType;
            jObject["id"] = messageModel.Id;
            jObject["ShopName"] = messageModel.ShopName;
            jObject["Content"] = messageModel.Content;
            jObject["OrderNumber"] = messageModel.OrderNumber;
            jObject["Group"] = messageModel.Group;
            jObject["Menus"] = jArray;

            return JsonConvert.SerializeObject(jObject);
        }
    }
}
