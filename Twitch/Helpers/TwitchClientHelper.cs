using RobertDev_Chatbot.Twitch.Connections;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;

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

    }
}
