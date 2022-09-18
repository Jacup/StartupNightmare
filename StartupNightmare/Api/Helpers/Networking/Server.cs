using System.Net;
using System.Net.Sockets;
using System.Text;
using StartupNightmare.Model.People;

namespace StartupNightmare.Api.Helpers.Networking;

internal sealed class Server
{
    #region Fields and Constants

    private static Server? instance;
    private static readonly object instanceLock = new();

    private readonly Socket serverSocket;
    private readonly List<Socket> clients;
    private readonly List<Player> connectedPlayers;
    private bool running;

    #endregion

    #region Constructors and Initializers

    private Server()
    {
        clients = new();
        connectedPlayers = new();
        serverSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    #endregion

    #region Public Methods

    public static Server GetInstance()
    {
        if (instance != null) return instance;

        lock (instanceLock)
        {
            instance ??= new();
        }

        return instance;
    }

    public List<Player> GetConnectedPlayers() => connectedPlayers;

    /// <summary>
    /// Start server.
    /// </summary>
    public void Start(IPAddress ipAddress, int portNumber)
    {
        IPEndPoint endPoint = new(ipAddress, portNumber);

        serverSocket.Bind(endPoint);
        serverSocket.Listen(5);

        running = true;
    }

    public Task<List<Socket>> OpenLobby(CancellationToken cancellationToken)
    {
        var msg = Encoding.UTF8.GetBytes("Hello, Client!\n");

        // To be refactored along with better Client/Player class support.
        var task = Task.Run(async () =>
        {
            Console.WriteLine($"Waiting for connections in thread: #{Environment.CurrentManagedThreadId}");

            var clientSocketTask = serverSocket.AcceptAsync(cancellationToken);

            while (!cancellationToken.IsCancellationRequested && running)
            {
                if (clientSocketTask.IsCompleted)
                {
                    var clientSocket = clientSocketTask.Result;

                    // Print some information about the remote client
                    Console.WriteLine($"Incoming connection from {clientSocket.RemoteEndPoint}, replying.");

                    // Send a reply (blocks)
                    clientSocket.Send(msg, SocketFlags.None);

                    // get data
                    var data = new byte[1024];

                    clientSocket.Receive(data, SocketFlags.None);
                    var strName = Encoding.UTF8.GetString(data);

                    Console.WriteLine($"ClientName: {strName}");
                    clients.Add(clientSocket);

                    clientSocketTask = serverSocket.AcceptAsync(cancellationToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            return clients;
        });

        return task;
    }

    #endregion

    #region Private Methods

    #endregion

}