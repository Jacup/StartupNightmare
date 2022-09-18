using StartupNightmare.Api.Helpers.Networking;
using StartupNightmare.Model;
using StartupNightmare.Model.People;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace StartupNightmare.Controller
{
    public class GameController
    {
        public Game game { get; set; }

        private static GameController? _instance;
        private static readonly object _lock = new();

        private GameController() { }

        public static GameController GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new GameController();
                }
            }

            return _instance;
        }

        public async Task InitializeGameAsync()
        {
            game = await GameCreator();
        }

        public static async Task<Game> GameCreator()
        {
            Game game = Game.GetInstance();
            Server server = Server.GetInstance();

            List<Socket> players = new();


            server.Run();

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                // Create a task to listen to keyboard key press
                var keyBoardTask = Task.Run(() =>
                {
                    Console.WriteLine($"Press enter to cancel #{Environment.CurrentManagedThreadId}");
                    Console.ReadKey();

                    // Cancel the task
                    cancellationTokenSource.Cancel();
                    Console.WriteLine("Cancelling. . .");
                });

                try
                {
                    var lobby = server.GetPlayers(cancellationTokenSource.Token);

                    players.AddRange(await lobby);

                    Console.WriteLine("Result {0}", players.Count);
                    Console.WriteLine($"Press enter to continue #{Environment.CurrentManagedThreadId}");
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine($"Task was cancelled. Number of players that joined game: {players.Count}");
                }

                await keyBoardTask;
            }

            return game;
        }

        private static void OpenConnection()
        {
            throw new NotImplementedException();
        }

        private static async Task<Player> GetPlayers()
        {
            // TODO: open socket for incoming players
            await Task.Delay(TimeSpan.FromSeconds(10));

            return new Player("Kuba");
        }
    }
}
