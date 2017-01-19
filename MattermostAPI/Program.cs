using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MattermostAPI
{
    public static class Program
    {

        #region CONST
        private const string ChannelId = "";
        private const string TeamId = "";
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
                string token = values.First();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // Get team
                string team = SendGetRequest(client, "/teams/all");

                // Get channel by name

                // Post message in a channel   
                       
                Console.WriteLine("Veuillez saisir un message a publier : ");
                string message = Console.ReadLine();
                PostMessage dataMessage = new PostMessage { message = message, channel_id = ChannelId };
                string parameters = "/teams/" + TeamId + "/channels/" + ChannelId + "/posts/create";

                try
                {
                    SendPostRequest(client, dataMessage, parameters);
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
        /// <returns></returns>
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
        /// <returns></returns>
        public static string SendGetRequest(HttpClient client, string parameters)
        {
            string request = HttpRequest + parameters;

            // Get
            HttpResponseMessage response = client.GetAsync(request).Result;

            // Response
            response.EnsureSuccessStatusCode();

            return  response.Content.ReadAsStringAsync().Result;
        }
    }
}
