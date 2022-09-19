using System.Threading.Channels;
using ConsoleLauncher;
using StartupNightmare.Controller;
using StartupNightmare.Resources;

namespace StartupNightmare;

internal class Program
{
    private static void Main()
    {
        InitializeDefaults();

        Welcome();

        GameController gc = GameController.GetInstance();

        gc.Initialize();

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

    private static void InitializeDefaults()
    {
        Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

    }

    private static void Welcome()
    {
        Console.WriteLine(Data.WelcomeMessage);


    }

    private static void Home()
    {

    }




}