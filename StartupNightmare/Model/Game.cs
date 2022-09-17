using StartupNightmare.Model.People;

namespace StartupNightmare.Model
{
    public class Game
    {        
        public List<Player>? players;

        private static Game? _instance;
        private static readonly object _lock = new();

        private Game() { }

        public static Game GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new Game();
                }
            }

            return _instance;
        }

        public void Show()
        {
            Console.WriteLine("List of Players:");
            Console.WriteLine(players);
        }
    }
}
