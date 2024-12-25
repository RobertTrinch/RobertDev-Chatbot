using RobertDev_Chatbot.Twitch.Connections;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Extensions;

namespace RobertDev_Chatbot.Twitch.Helpers
{
    public class TwitchClientHelper
    {
        public static void SendMessage(string message)
        {
            //TODO: cooldown?
            ClientConnection.client.SendMessage(Config.ChannelUsername, message);
            Log.Information($"[Twitch Message Sent] {message}");
        }

        public static void TimeoutUser(string username, int seconds, string message)
        {
            ClientConnection.client.TimeoutUser(Config.ChannelUsername, username, TimeSpan.FromSeconds(seconds), message);  
            Log.Information($"[Twitch Moderation] Timed {username} out for {seconds} seconds: {message}");
        }

        public static void BanUser(string username, string message)
        {
            ClientConnection.client.BanUser(Config.ChannelUsername, username, message);
            Log.Information($"[Twitch Moderation] Banned {username}: {message}");
        }

    }
}
