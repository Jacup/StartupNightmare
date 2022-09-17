using System.Net.Sockets;
using System.Text;

namespace StartupNightmare.Api.Helpers.Networking
{
    internal class StateObject
    {
        // Size of receive buffer.
        public const int BufferSize = 1024;

        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new();

        // Client socket.
        public Socket? workSocket = null;
    }
}
