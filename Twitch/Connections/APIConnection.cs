using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;
using TwitchLib.Client.Models;

namespace RobertDev_Chatbot.Twitch.Connections
{
    class APIConnection
    {
        public static TwitchAPI api;

        public static void StartAPI()
        {
            api = new TwitchAPI();
            api.Settings.ClientId = Config.APIClientId;
            api.Settings.AccessToken = Config.BotAccessToken;
        }

        public static async Task RefreshToken()
        {
            Log.Information("[API Connection] Refreshing Token - " + Config.BotAccessToken);
            var refresh = await api.Auth.RefreshAuthTokenAsync(Config.BotRefreshToken, Config.APIClientSecret);

            using var db = new Database.DatabaseContext();
            var config = db.Config.FirstOrDefault();
            config.BotAccessToken = refresh.AccessToken; // database
            Config.BotAccessToken = refresh.AccessToken; // config.cs
            db.SaveChanges();

            Log.Information("[API Connection] New Token - " + Config.BotAccessToken);
            ClientConnection.client.SetConnectionCredentials(new ConnectionCredentials(Config.BotUsername, Config.BotAccessToken)); // client connection
        }
    }
}
