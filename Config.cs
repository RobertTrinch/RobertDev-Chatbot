using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobertDev_Chatbot
{
    public class Config
    {
        public static string BotUsername { get; set; }
        public static string BotAccessToken { get; set; }
        public static string BotRefreshToken { get; set; }

        public static string ChannelUsername { get; set; }

        public static string APIClientId { get; set; }
        public static string APIClientSecret { get; set; }

        public static void GetConfig()
        {
            var appSettings = ConfigurationManager.AppSettings;
            BotUsername = appSettings["botUsername"];
            BotAccessToken = appSettings["botAccessToken"];
            BotRefreshToken = appSettings["botRefreshToken"];
            ChannelUsername = appSettings["channelUsername"];
            APIClientId = appSettings["apiClientId"];
            APIClientSecret = appSettings["apiClientSecret"];

            Log.Information("[Config] Config Loaded");
        }

    }
}
