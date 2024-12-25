using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Api;

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
            Configuration configFile= ConfigurationManager.OpenMachineConfiguration();
            Config.BotAccessToken = refresh.AccessToken;
            var appSettings = ConfigurationManager.AppSettings;
            appSettings["botAccessToken"] = refresh.AccessToken;
            configFile.Save(ConfigurationSaveMode.Modified);

            Log.Information("[API Connection] New Token - " + Config.BotAccessToken);
        }
    }
}
