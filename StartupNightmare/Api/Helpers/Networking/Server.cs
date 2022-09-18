using System.Net;
using System.Net.Sockets;
using System.Text;

namespace StartupNightmare.Api.Helpers.Networking
{
    internal sealed class Server
    {
        public int PortNumber = 6000;
        public bool Running = false;
        public Socket ServerSocket;
        public List<Socket> clients = new();

        private static Server? _instance;
        private static readonly object _lock = new();

        private Server()
        {
        }

        public static Server GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new Server();
                }
            }

            return _instance;
        }

        public void InterruptHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Received SIGINT, shutting down server.");

            // Cleanup
            Running = false;
            ServerSocket.Shutdown(SocketShutdown.Both);
            ServerSocket.Close();
        }

        public void Run()
        {
            // Set the endpoint options
            IPEndPoint serv = new(IPAddress.Any, PortNumber);

            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(serv);

            // Start listening for connections (queue of 5 max)
            ServerSocket.Listen(5);

            // Setup the Ctrl-C
            Console.CancelKeyPress += InterruptHandler;
            Running = true;
            Console.WriteLine("Running the TCP server.");

        }
        public Task<decimal> LongRunningCancellableOperation(int loop, CancellationToken cancellationToken)
        {
            Task<decimal> task = null;

            // Start a task and return it
            task = Task.Run(() =>
            {
                decimal result = 0;

                // Loop for a defined number of iterations
                for (int i = 0; i < loop; i++)
                {
                    // Check if a cancellation is requested, if yes,
                    // throw a TaskCanceledException.

                    if (cancellationToken.IsCancellationRequested)
                        throw new TaskCanceledException(task);

                    // Do something that takes times like a Thread.Sleep in .NET Core 2.
                    Thread.Sleep(10);
                    result += i;
                }

                return result;
            });

            return task;
        }

        internal Task<List<Socket>> GetPlayers(CancellationToken cancellationToken)
        {
            byte[] msg = Encoding.UTF8.GetBytes("Hello, Client!\n");
            Task<List<Socket>> task;

            // Main loop
            task = Task.Run(async () =>
            {
                Socket clientSocket;
                Console.WriteLine($"Waiting for connections in thread: #{Environment.CurrentManagedThreadId}");
                ValueTask<Socket> ClientSocketTask;

                ClientSocketTask = ServerSocket.AcceptAsync(cancellationToken);

                while (!cancellationToken.IsCancellationRequested && Running)
                {


                    if (ClientSocketTask.IsCompleted)
                    {
                        clientSocket = ClientSocketTask.Result;
                        // Print some infor about the remote client
                        Console.WriteLine($"Incoming connection from {clientSocket.RemoteEndPoint}, replying.");

                        // Send a reply (blocks)
                        clientSocket.Send(msg, SocketFlags.None);

                        // get data
                        byte[] data = new byte[1024];

                        clientSocket.Receive(data, SocketFlags.None);
                        var strName = Encoding.UTF8.GetString(data);

                        Console.WriteLine($"ClientName: {strName}");
                        clients.Add(clientSocket);

                        ClientSocketTask = ServerSocket.AcceptAsync(cancellationToken);
                    }


                    await Task.Delay(TimeSpan.FromSeconds(1));
                }

                return clients;
            });

            return task;
        }




















    }

}