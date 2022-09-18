using ConsoleLauncher;
using ConsoleLauncher.Layout;
using StartupNightmare.Api;
using StartupNightmare.Controller;
using StartupNightmare.Model;

namespace StartupNightmare
{
    internal class Program
    {
        static void Main()
        {

            GameController gc = GameController.GetInstance();

            gc.InitializeGameAsync();
            Console.WriteLine("Game hosted.");

            //Startup.Configure();

            //List<Option> options = new()
            //{
            //    new Option("Start new game", () => GameController.InitializeGame()),
            //    new Option("Join game"),
            //    new Option("Show tutorial"),
            //    new Option("Exit", () => Environment.Exit(0))
            //};

            //Launcher.Menu(options);

            Console.ReadKey();
        }


    }
}