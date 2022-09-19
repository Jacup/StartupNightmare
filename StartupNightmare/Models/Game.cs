using System.Net.Sockets;
using StartupNightmare.Model.People;

namespace StartupNightmare.Model
{
    public class Game
    {        
        public List<Socket> Players = new();

        private static Game? instance;
        private static readonly object instanceLock = new();

        private Game() { }

        #region Public Methods

        public static Game GetInstance()
        {
            if (instance != null) return instance;

            lock (instanceLock)
            {
                instance ??= new();
            }

            return instance;
        }
        

        #endregion
    }
}
