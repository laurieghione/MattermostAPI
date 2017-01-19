using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MattermostAPI
{
    public static class Program
    {

        #region CONST
        private const string TeamName = "";
        private const string ChannelName = "test api";
        private const string HttpRequest = "https://tchat.com/api/v3";
        #endregion  

        public static void Main()
        {
            Start();
        }

        public static void Start()
        {

            // Get Authentification informations
            Console.WriteLine("Veuillez saisir votre login Mattermost : ");
            string loginId = Console.ReadLine();
            Console.WriteLine("Veuillez saisir votre password Mattermost : ");
            ConsoleColor oldFore = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;
            string password = Console.ReadLine();
            Console.ForegroundColor = oldFore;

            // Headers
            HttpClient client = new HttpClient();
            IEnumerable<string> values;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Get Token by login informations
            PostLogin data = new PostLogin { login_id = loginId, password = password };
            var userLogin = SendPostRequest(client, data, "/users/login");

            if (userLogin.Headers.TryGetValues("Token", out values))
            {
                // Get user by login
                string getUser = SendGetRequest(client, "/users/me");
                User user = JsonConvert.DeserializeObject<User>(getUser);

                Console.WriteLine("Bonjour " + user.first_name + " " + user.last_name  + " Authentification réussie");

                string token = values.First();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // Get team by name
                Console.WriteLine("Récupération des informations de la team");

                string getTeam = SendGetRequest(client, "/teams/name/" + TeamName);
                Team team = JsonConvert.DeserializeObject<Team>(getTeam);

                Console.WriteLine("Identifiant de la team " + TeamName + "  est : " + team.id);

                // Get channel by name
                Console.WriteLine("Récupération des informations du channel");

                string getChannel = SendGetRequest(client, "/teams/" + team.id + "/channels/name/" + ChannelName);
                Channel channel = JsonConvert.DeserializeObject<Channel>(getChannel);

                Console.WriteLine("Identifiant du channel " + ChannelName + "  est : " + channel.id);

                // Post message in a channel   
                Console.WriteLine("Veuillez saisir un message a publier : ");
                string message = Console.ReadLine();
                PostMessage dataMessage = new PostMessage { message = message, channel_id = channel.id };
                string parameters = "/teams/" + team.id + "/channels/" + channel.id + "/posts/create";

                try
                {
                    SendPostRequest(client, dataMessage, parameters);
                    Console.WriteLine("Message publié dans le channel");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Erreur : " + e);
                }

            }
        }

        /// <summary>
        /// Post a request
        /// </summary>
        /// <param name="client"> Http client headers</param>
        /// <param name="data"> Payload to post</param>
        /// <param name="parameters">Add parameters in request</param>
        /// <returns>HttpResponseMessage response</returns>
        public static HttpResponseMessage SendPostRequest(HttpClient client, object data, string parameters)
        {
            string request = HttpRequest + parameters;

            var json = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(json);

            // Post
            HttpResponseMessage response = client.PostAsync(request, httpContent).Result;

            // Response
            response.EnsureSuccessStatusCode();

            return response;
        }

        /// <summary>
        /// Send a get request
        /// </summary>
        /// <param name="client"> Http client headers</param>
        /// <param name="parameters">Add parameters in request</param>
        /// <returns>string response</returns>
        public static string SendGetRequest(HttpClient client, string parameters)
        {
            string request = HttpRequest + parameters;

            // Get
            HttpResponseMessage response = client.GetAsync(request).Result;

            // Response
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
