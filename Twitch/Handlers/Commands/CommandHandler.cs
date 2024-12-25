using RobertDev_Chatbot.Twitch.Connections;
using RobertDev_Chatbot.Twitch.Handlers.Users;
using RobertDev_Chatbot.Twitch.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace RobertDev_Chatbot.Twitch.Handlers.Commands
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
                case "!delcmd" when e.ChatMessage.IsBroadcaster || e.ChatMessage.IsModerator:
                case "!delcom" when e.ChatMessage.IsBroadcaster || e.ChatMessage.IsModerator:
                    RemoveCommand(e);
                    break;
                case "!editcmd" when e.ChatMessage.IsBroadcaster || e.ChatMessage.IsModerator:
                case "!editcom" when e.ChatMessage.IsBroadcaster || e.ChatMessage.IsModerator:
                    EditCommand(e);
                    break;
                //TODO: support to tag people and check theirs
                case "!points":
                    TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> You have {Points.GetUserPoints(e.ChatMessage.DisplayName.ToLower()):N0} points!");
                    break;
                case "!messages":
                    TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> You have sent {Points.GetUserMessages(e.ChatMessage.DisplayName):N0} messages!");
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
            Log.Information("[Twitch Commands] Handled command: " + command);
        }

        public static void AddCommand(OnMessageReceivedArgs e)
        {
            // Check length
            if (e.ChatMessage.Message.Split(' ').Length < 3)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Usage: !addcmd [command] [message]");
                return;
            }

            // Check if exists
            using var db = new Database.DatabaseContext();
            string command = e.ChatMessage.Message.ToLower().Split(' ')[1];
            var commandCheck = db.Commands.FirstOrDefault(x => x.Command == command);

            if (commandCheck != null)
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
            Log.Information("[Twitch Commands] Added command: " + e.ChatMessage.Message.Split(' ')[1]);
        }

        public static void RemoveCommand(OnMessageReceivedArgs e)
        {
            // Check length
            if (e.ChatMessage.Message.Split(' ').Length < 2)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Usage: !removecmd [command]");
                return;
            }

            // Check if exists
            using var db = new Database.DatabaseContext();
            string command = e.ChatMessage.Message.ToLower().Split(' ')[1];
            var commandCheck = db.Commands.FirstOrDefault(x => x.Command == command);

            if (commandCheck == null)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Command '{command}' does not exist.");
                return;
            }

            // Remove from database
            db.Remove(commandCheck);
            TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Command '{command}' has been removed.");
            db.SaveChanges();
            Log.Information("[Twitch Commands] Removed command: " + command);
        }

        public static void EditCommand(OnMessageReceivedArgs e)
        {
            // Check length
            if (e.ChatMessage.Message.Split(' ').Length < 3)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Usage: !editcmd [command] [message]");
                return;
            }

            // Check if exists
            using var db = new Database.DatabaseContext();
            string command = e.ChatMessage.Message.ToLower().Split(' ')[1];
            var commandCheck = db.Commands.FirstOrDefault(x => x.Command == command);

            if (commandCheck == null)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Command '{command}' does not exist.");
                return;
            }

            // Edit in database
            commandCheck.Message = e.ChatMessage.Message.Remove(0, e.ChatMessage.Message.Split(' ')[0].Length + e.ChatMessage.Message.Split(' ')[1].Length + 2);
            TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Command '{command}' has been edited.");
            db.SaveChanges();
            Log.Information("[Twitch Commands] Edited command: " + command);
        }

    }
}
