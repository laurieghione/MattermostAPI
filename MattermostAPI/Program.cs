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
        private const string TeamName = "npsix";
        private const string HttpRequest = "https://tchat.mailperformance.com/api/v3";
        #endregion  

        public static void Main(string[] args)
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

            try
            {
                var userLogin = SendPostRequest(client, data, "/users/login");

                if (userLogin.Headers.TryGetValues("Token", out values))
                {
                    // Put header authentification
                    string token = values.First();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    Console.WriteLine("Authentification réussie");

                    // Get user by login
                    string getUser = SendGetRequest(client, "/users/me");
                    User user = JsonConvert.DeserializeObject<User>(getUser);

                    Console.WriteLine(string.Format("Bonjour {0} {1}", user.first_name, user.last_name));

                    // Get team by name
                    Console.WriteLine("Récupération des informations de la team");

                    string getTeam = SendGetRequest(client, string.Format("/teams/name/{0}", TeamName));
                    Team team = JsonConvert.DeserializeObject<Team>(getTeam);

                    Console.WriteLine(string.Format("Identifiant de la team {0} est : {1}", TeamName, team.id));

                    // Get channel by name
                    Console.WriteLine("Veuillez saisir le nom du channel");
                    string channelName = Console.ReadLine();
                    Console.WriteLine("Récupération des informations du channel");

                    string getChannel = SendGetRequest(client, string.Format("{0}/teams/{1}/channels/name/{2}", client, team.id, channelName));
                    Channel channel = JsonConvert.DeserializeObject<Channel>(getChannel);

                    Console.WriteLine(string.Format("Identifiant du channel {0} est : {1}", channelName, channel.id));

                    // Post message in a channel   
                    Console.WriteLine("Veuillez saisir un message a publier : ");
                    string message = Console.ReadLine();
                    PostMessage dataMessage = new PostMessage { message = message, channel_id = channel.id };
                    string parameters = string.Format("/teams/{0}/channels/{1}/posts/create", team.id, channel.id);

                    try
                    {
                        SendPostRequest(client, dataMessage, parameters);
                        Console.WriteLine("Message publié dans le channel");
                        Console.ReadKey();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Erreur " + e);
                        Console.ReadKey();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur d'authentification" + e);
                Console.ReadKey();
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
