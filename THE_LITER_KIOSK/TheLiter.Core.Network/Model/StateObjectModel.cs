using System.Net.Sockets;
using System.Text;

namespace TheLiter.Core.Network.Model
{
    public class StateObjectModel
    {
        public Socket workSocket = null;
        public const int BufferSize = 4600;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }
}
