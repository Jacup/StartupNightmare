using StartupNightmare.Api.Helpers.Networking;
using StartupNightmare.Model;
using System.Net;

namespace StartupNightmare.Controller
{
    public class GameController
    {
        private Game? game;

        private static GameController? instance;
        private static readonly object instanceLock = new();

        private GameController() { }

        public static GameController GetInstance()
        {
            if (instance != null) return instance;
            lock (instanceLock)
            {
                instance ??= new();
            }

            return instance;
        }

        public async void InitializeGameAsync()
        {
            await HostNewGame();
        }

        public async Task HostNewGame()
        {
            game = Game.GetInstance();

            var server = Server.GetInstance();
            const int portNumber = 6000;

            server.Start(IPAddress.Any, portNumber);

            using var cancellationTokenSource = new CancellationTokenSource();

            var userAction = Task.Run(() => WaitForUserAction(cancellationTokenSource));
            await server.OpenLobby(cancellationTokenSource.Token);

            await userAction;

            game.Players.AddRange(server.GetConnectedPlayers());
        }

        private static void WaitForUserAction(CancellationTokenSource cancellationTokenSource)
        {
            Console.WriteLine($"Press enter to cancel #{Environment.CurrentManagedThreadId}");
            Console.ReadKey();

            // Cancel the task
            cancellationTokenSource.Cancel();
            Console.WriteLine("Cancelling. . .");
        }

        #region Private Methods



        #endregion

    }
}
