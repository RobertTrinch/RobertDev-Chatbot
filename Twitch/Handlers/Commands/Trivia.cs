using Newtonsoft.Json;
using RobertDev_Chatbot.Twitch.Handlers.Users;
using RobertDev_Chatbot.Twitch.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TwitchLib.Api.Core.Extensions.System;
using TwitchLib.Client.Events;

namespace RobertDev_Chatbot.Twitch.Handlers.Commands
{
    public class Trivia
    {
        private static Dictionary<string, ActiveTrivia> activeTriviaUsers = new();

        public static async void HandleTriviaQuestionCommand(OnMessageReceivedArgs e)
        {

            if(activeTriviaUsers.ContainsKey(e.ChatMessage.Username))
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> You already have an active trivia question. Please answer that one first.");
                return;
            }

            if (e.ChatMessage.Message.Split(' ').Count() != 2)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Usage: !trivia <amount>");
                return;
            }

            int betAmount = Int32.TryParse(e.ChatMessage.Message.Split(' ')[1].Replace(",", ""), out int result) ? result : 0;
            if (betAmount == 0)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> You can't spend nothing silly! (either you put not a number or 0)");
                return;
            }
            if (betAmount < 100)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> You need to spend more than 100 points!");
                return;
            }
            if (Points.GetUserPoints(e.ChatMessage.Username) < betAmount)
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> You don't have enough points to spend that amount!");
                return;
            }

            // Get trivia question from the API
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://opentdb.com/api.php?amount=1";
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    TriviaQuestion triviaQuestion = JsonConvert.DeserializeObject<TriviaQuestion>(json);
                    // Shuffle the answers (thank you copilot for this amazing one-liner)
                    triviaQuestion.results[0].incorrect_answers = triviaQuestion.results[0].incorrect_answers.Append(triviaQuestion.results[0].correct_answer).OrderBy(x => Guid.NewGuid()).ToArray();
                    string answers = string.Join(" ▪️ ", triviaQuestion.results[0].incorrect_answers);
                    TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> {HttpUtility.HtmlDecode(triviaQuestion.results[0].question)} ❓ --- Your answers are: {"▪️ " + answers} PogChamp Place your answer by doing !answer <answer> (Full answer!)");
                    // Add user to active trivia users
                    activeTriviaUsers.Add(e.ChatMessage.Username, new ActiveTrivia { Answer = triviaQuestion.results[0].correct_answer, BetAmount = betAmount });
                }
                else
                {
                    TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> There was an error getting the trivia question. Please try again later.");
                    return;
                }
            }
        }

        public static void HandleTriviaAnswerCommand(OnMessageReceivedArgs e)
        {
            if (activeTriviaUsers.ContainsKey(e.ChatMessage.Username))
            {
                if (e.ChatMessage.Message.Split(' ').Count() < 2)
                {
                    TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Usage: !answer <answer>");
                    return;
                }
                Console.WriteLine(activeTriviaUsers[e.ChatMessage.Username].Answer.ToLower() + "----" + e.ChatMessage.Message.Remove(0, e.ChatMessage.Message.Split(' ')[0].Count() + 1).ToLower());

                if (activeTriviaUsers[e.ChatMessage.Username].Answer.ToLower() == e.ChatMessage.Message.Remove(0, e.ChatMessage.Message.Split(' ')[0].Count() + 1).ToLower())
                {
                    Points.AddUserPoints(e.ChatMessage.Username, (int)(activeTriviaUsers[e.ChatMessage.Username].BetAmount * 1.5));
                    TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Correct! PogChamp You have won {(int)(activeTriviaUsers[e.ChatMessage.Username].BetAmount * 1.5):N0} points!");
                }
                else
                {
                    Points.RemoveUserPoints(e.ChatMessage.Username, activeTriviaUsers[e.ChatMessage.Username].BetAmount);
                    TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> Incorrect, the correct answer was {activeTriviaUsers[e.ChatMessage.Username].Answer}! You have lost your {activeTriviaUsers[e.ChatMessage.Username].BetAmount:N0} points!");
                }
                activeTriviaUsers.Remove(e.ChatMessage.Username);
                return;
            }
            else
            {
                TwitchClientHelper.SendMessage($"@{e.ChatMessage.DisplayName} -> You don't have an active trivia question. Please start one first by doing !trivia <amount>");
                return;
            }
        }

        private class TriviaQuestion
        {
            public List<TriviaResult> results { get; set; }
        }

        private class TriviaResult
        {
            public string question { get; set; }
            public string[] incorrect_answers { get; set; }
            public string correct_answer { get; set; }
        }

        public class ActiveTrivia
        {
            public string Answer { get; set; }
            public int BetAmount { get; set; }
        }
    }
}
