using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sportner.Messages.UserMessages;
using Sportner.Models;
using System.IO;
using System.Runtime.Serialization.Json;
using Sportner.Dto;
using Sportner.Messages.EventMessages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace RestAPIClient
{
    public class Client
    {
        private RestClient client = new RestClient("http://sportner.azurewebsites.net");
        public async Task<List<UserDto>> GetAllUsers()
        {
            var request = new RestRequest("api/Users", System.Net.Http.HttpMethod.Get);

            var response = await client.Execute(request);

            JsonDeserializer deserial = new JsonDeserializer();
            var users = deserial.Deserialize<GetAllUsersResponse>(response);
            return users.Users;
        }

        public async Task<List<EventWithUserDto>> GetAllEvents()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://sportner.azurewebsites.net");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                GetFilteredEventsRequest requestData = new GetFilteredEventsRequest
                {
                    City = "Vilnius"
                };

                var json_object = JsonConvert.SerializeObject(requestData);
                var response = await client.PostAsync("api/GetFilteredEvents", new StringContent(json_object.ToString(), Encoding.UTF8, "application/json"));


                string jsonMessage;
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    jsonMessage = new StreamReader(responseStream).ReadToEnd();
                }

                GetFilteredEventsResponse jsonResponse = JsonConvert.DeserializeObject<GetFilteredEventsResponse>(jsonMessage);
                
                return jsonResponse.EventsWithUsers;
            }
        }

        public async Task<HttpStatusCode> AddEvent(EventDto eventIns)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://sportner.azurewebsites.net");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                AddEventRequest requestData = new AddEventRequest
                {
                    Event = eventIns
                };

                var json_object = JsonConvert.SerializeObject(requestData);

                var response = await client.PostAsync("api/Events", new StringContent(json_object.ToString(), Encoding.UTF8, "application/json"));
                return response.StatusCode;
            }
        }
    }
}
