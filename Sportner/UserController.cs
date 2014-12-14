using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.Deserializers;
using Sportner.Dto;
using Sportner.Messages.EventMessages;
using Sportner.Messages.UserMessages;

namespace Sportner
{
    public class UserController
    {
        public async void InsertUser(UserDto user)
        {
            using (var client = new HttpClient())
            {
                // New code:
                client.BaseAddress = new Uri("http://sportner.azurewebsites.net");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                AddUserRequest requestData = new AddUserRequest
                {
                    User = user
                };

                var json_object = JsonConvert.SerializeObject(requestData);

                var response = await client.PostAsync("api/Users", new StringContent(json_object.ToString(), Encoding.UTF8, "application/json"));
            }
        }

        public async Task<IsPasswordCorrectResponse> Login(string email, string password)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://sportner.azurewebsites.net");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                IsPasswordCorrectRequest requestData = new IsPasswordCorrectRequest
                {
                    Email = email,
                    Password = password
                };

                var json_object = JsonConvert.SerializeObject(requestData);

                var response =
                    await
                        client.PostAsync("api/Users/Authorize",
                            new StringContent(json_object.ToString(), Encoding.UTF8, "application/json"));


                string jsonMessage;
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    jsonMessage = new StreamReader(responseStream).ReadToEnd();
                }

                IsPasswordCorrectResponse jsonResponse =
                    JsonConvert.DeserializeObject<IsPasswordCorrectResponse>(jsonMessage);

                return jsonResponse;
            }

        }

        public async Task<List<UserDto>> GetUsers()
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://sportner.azurewebsites.net");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                GetAllUserRequest requestData = new GetAllUserRequest();

                var json_object = JsonConvert.SerializeObject(requestData);

                var response = await client.GetAsync("api/Users");


                string jsonMessage;
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    jsonMessage = new StreamReader(responseStream).ReadToEnd();
                }

                GetAllUsersResponse jsonResponse = JsonConvert.DeserializeObject<GetAllUsersResponse>(jsonMessage);

                return jsonResponse.Users;
            }


        }
    }
}
