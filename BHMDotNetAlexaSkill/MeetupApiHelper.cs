using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BHMDotNetAlexaSkill.Entities;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace BHMDotNetAlexaSkill
{
    public class MeetupApiHelper
    {
        //Base URL for accesing the Meetup.com API
        const string BASE_URL = "https://api.meetup.com/";
        //URL for accessing events
        const string EVENTS_URL = "/Birmingham-NET-Meetup/events";
        //Access token if you use authentication
        private string _accessToken;
        //Address of your Web Api
        private string _serviceAddress;

        //Constructor:
        public MeetupApiHelper(string accessToken)
        {
            _accessToken = accessToken;
        }

        public async Task<Event> GetUpcomingEvent()
        {
            Event upcomingEvent = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
                    var data = await client.GetAsync(string.Concat(BASE_URL, EVENTS_URL));
                    var jsonResponse = await data.Content.ReadAsStringAsync();

                    if (jsonResponse != null)
                    {
                        
                        upcomingEvent = JsonConvert.DeserializeObject<Event>(jsonResponse);
                    }

                    return upcomingEvent;
                }
            }

            catch (WebException exception)
            {
                throw new WebException("An error has occurred while calling GetUpcomingEvents method: " + exception.Message);
            }
        } 
    }
}
