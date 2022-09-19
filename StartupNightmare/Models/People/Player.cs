using System.Net.Sockets;

namespace StartupNightmare.Model.People
{
    public class Player : Socket
    {
        public string Name { get; set; }

        public Player(string name) : base(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
        {
            Name = name;
        }
    }
}
