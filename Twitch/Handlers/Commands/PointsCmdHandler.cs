using RobertDev_Chatbot.Twitch.Handlers.Users;
using RobertDev_Chatbot.Twitch.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace RobertDev_Chatbot.Twitch.Handlers.Commands
{
    public class PointsCmdHandler
    {

        public static void GetUserPoints_Command(OnMessageReceivedArgs e)
        {
            if(e.ChatMessage.Message.ToLower().Split(' ').Count() != 2)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> You have {Points.GetUserPoints(e.ChatMessage.DisplayName.ToLower().Replace("@", "")):N0} points!");
                return;
            }

            var user = e.ChatMessage.Message.ToLower().Split(' ')[1];
            TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> {user} has {Points.GetUserPoints(user.ToLower()):N0} points!");
        }
    }
}
