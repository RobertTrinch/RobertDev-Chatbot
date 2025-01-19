using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Events;

namespace RobertDev_Chatbot.Twitch.Handlers.Users
{
    public class UserHandler
    {

        public static void OnMessage_CheckUser(OnMessageReceivedArgs e)
        {
            using var db = new Database.DatabaseContext();
            var user = db.Users.FirstOrDefault(x => x.UserID == e.ChatMessage.UserId);
            if (user == null)
            {
                db.Users.Add(new Database.Users
                {
                    UserID = e.ChatMessage.UserId,
                    Username = e.ChatMessage.DisplayName,
                    MessageCount = 1,
                    Points = 2500
                });
                Log.Information("[Twitch Users] Added new user to database: " + e.ChatMessage.DisplayName);
            }
            else
            {
                user.MessageCount++;
                user.Points = user.Points + 5;
                Log.Information("[Twitch Users] Updated user in database: " + e.ChatMessage.DisplayName);
            }
            db.SaveChanges();
        }

    }
}
