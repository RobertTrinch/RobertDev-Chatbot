using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertDev_Chatbot.Twitch.Handlers.Users
{
    public class Points
    {

        public static int GetUserPoints(string username)
        {
            using var db = new Database.DatabaseContext();
            var user = db.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
            if (user == null)
            {
                return 0;
            }
            return user.Points;
        }

        public static bool AddUserPoints(string username, int amount)
        {
            using var db = new Database.DatabaseContext();
            var user = db.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
            if (user == null)
            {
                return false;
            }
            user.Points += amount;
            db.SaveChanges();
            return true;
        }

        public static bool RemoveUserPoints(string username, int amount)
        {
            using var db = new Database.DatabaseContext();
            var user = db.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
            if (user == null)
            {
                return false;
            }
            user.Points -= amount;
            db.SaveChanges();
            return true;
        }

        public static int GetUserMessages(string username)
        {
            using var db = new Database.DatabaseContext();
            var user = db.Users.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());
            if (user == null)
            {
                return 0;
            }
            return user.MessageCount;
        }

    }
}
