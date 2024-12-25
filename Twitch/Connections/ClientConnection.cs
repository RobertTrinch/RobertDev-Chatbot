using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace RobertDev_Chatbot.Twitch.Connections
{
    class ClientConnection
    {

        TwitchClient client;
        public ClientConnection()
        {
            ConnectionCredentials credentials = new ConnectionCredentials(Config.BotUsername, Config.BotAccessToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, Config.ChannelUsername);

            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnWhisperReceived += Client_OnWhisperReceived;
            client.OnConnected += Client_OnConnected;

            client.Connect();
        }
        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Log.Information("[Twitch Client] Connected");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Log.Information($"[Twitch Client] Joined channel: {e.Channel}");
            client.SendMessage(e.Channel, "Connected");
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            Log.Information($"[Twitch Message] ({e.ChatMessage.UserId}) {e.ChatMessage.DisplayName}: {e.ChatMessage.Message}");
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            Log.Information($"[Twitch Whisper] ({e.WhisperMessage.UserId}) {e.WhisperMessage.Username}: {e.WhisperMessage.Message}");
            client.SendWhisper(e.WhisperMessage.Username, $"Hey {e.WhisperMessage.Username}! This is an automated message, this bot can be used only in the channel's chat and not in whispers. :)");
        }

    }
}
