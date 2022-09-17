using StartupNightmare.Api.Interfaces;
using StartupNightmare.Controller;
using StartupNightmare.Model.People;
using System.Net.Sockets;

namespace StartupNightmare.Api.Helpers.Networking
{
    internal sealed class Server : IServer
    {
        // Listener for new incoming connections.
        private TcpListener _listener;

        // Clients objects
        private List<TcpClient> _clients = new();
        private List<TcpClient> _waitingLobby = new();


        public readonly string Name;
        public readonly int Port;
        public bool Running { get; private set; }

        private Server (string name, int port)
        {
        }





        public async Task<Player> GetPlayer(TcpClient tcpClient)
        {
        }
    }





}
