using RobertDev_Chatbot.Twitch.Connections;
using RobertDev_Chatbot.Twitch.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace RobertDev_Chatbot.Twitch.Commands
{
    class CommandHandler
    {

        public static void HandleMessage(OnMessageReceivedArgs e)
        {
            switch (e.ChatMessage.Message.ToLower().Split(' ')[0])
            {
                case "!hello":
                    TwitchClientHelper.SendMessage($"Hello {e.ChatMessage.DisplayName}!");
                    break;
                case "!addcmd" when e.ChatMessage.IsBroadcaster || e.ChatMessage.IsModerator:
                case "!addcom" when e.ChatMessage.IsBroadcaster || e.ChatMessage.IsModerator:
                    AddCommand(e);
                    break;
                default:
                    GetCommand(e.ChatMessage.DisplayName, e.ChatMessage.Message.ToLower().Split(' ')[0]);
                    break;
            }
        }

        public static void GetCommand(string user, string command)
        {
            using var db = new Database.DatabaseContext();
            var cmd = db.Commands.FirstOrDefault(x => x.Command == command);
            if (cmd != null)
            {
                cmd.TimesUsed++;
                TwitchClientHelper.SendMessage(cmd.Message.Replace("[user]", user).Replace("[count]", $"{cmd.TimesUsed}"));
                db.SaveChanges();
            }
        }

        public static void AddCommand(OnMessageReceivedArgs e)
        {
            // Check length
            if(e.ChatMessage.Message.Split(' ').Length < 3)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Usage: !addcmd [command] [message]");
                return;
            }

            // Check if exists
            using var db = new Database.DatabaseContext();
            string command = e.ChatMessage.Message.ToLower().Split(' ')[0];
            var commandCheck = db.Commands.FirstOrDefault(x => x.Command == command);

            if(commandCheck == null)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Command '{command}' already exists.");
                return;
            }

            // Add to database
            db.Add(new Database.Commands
            {
                Command = e.ChatMessage.Message.Split(' ')[1].ToLower(),
                Message = e.ChatMessage.Message.Remove(0, e.ChatMessage.Message.Split(' ')[0].Length + e.ChatMessage.Message.Split(' ')[1].Length + 2),
                TimesUsed = 0
            });
            TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Command '{e.ChatMessage.Message.Split(' ')[1]}' has been added.");
            db.SaveChanges();
        }

    }
}
