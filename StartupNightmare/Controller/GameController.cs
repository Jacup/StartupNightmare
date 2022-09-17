using StartupNightmare.Model;
using StartupNightmare.Model.People;
using System.Configuration;

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
            game = await CreateGame();
        }

        public static async Task<Game> CreateGame()
        {
            Game game = Game.GetInstance();
            List<Player> players = new();

            // open connection for incoming players
            //OpenConnection();

            // wait for players
            //while (players.Count != Convert.ToInt16(ConfigurationManager.AppSettings["MaxNumberOfPlayers"]))
            while (players.Count != 1)
            {
                players.Add(await GetPlayer());
            }

            // start game if all players joined or host started game
            game.players = players;

            return game;
        }

        private static void OpenConnection()
        {
            throw new NotImplementedException();
        }

        private static async Task<Player> GetPlayer()
        {
            // TODO: open socket for incoming players
            await Task.Delay(TimeSpan.FromSeconds(10));

            return new Player("Kuba");
        }
    }
}
