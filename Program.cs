// See https://aka.ms/new-console-template for more information
using RobertDev_Chatbot;
using RobertDev_Chatbot.Connections.Twitch;

internal class Program
{
    private static void Main(string[] args)
    {
        // Get config
        Config.GetConfig();

        // Twitch connections
        ClientConnection clientConnection = new ClientConnection();

        Console.ReadLine();
    }
}