using System.Net.Sockets;

namespace THE_LITER_KIOSK.Network
{
    public static class TcpHelper
    {
        public static Socket SocketClient { get; set; }
        public static bool IsAvailable { get; set; }
        public static string ReceiveMessage { get; set; }

        public static void InitializeClient()
        {
            SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
