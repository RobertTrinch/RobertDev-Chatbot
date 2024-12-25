// See https://aka.ms/new-console-template for more information
using RobertDev_Chatbot;
using RobertDev_Chatbot.Twitch.Connections;
using Serilog;

internal class Program
{
    private static void Main(string[] args)
    {
        // Logger
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/log.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        // Get config
        Config.GetConfig();

        // Twitch connections
        ClientConnection clientConnection = new ClientConnection();

        Console.ReadLine();
    }
}