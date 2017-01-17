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
        const string EVENTS_URL = "Birmingham-NET-Meetup/events";
        //Access token if you use authentication
        private string _accessToken;
        //Address of your Web Api
        private string _serviceAddress;

        //Constructor:
        public MeetupApiHelper(string accessToken)
        {
            _accessToken = accessToken;
        }

        public async Task<RootEventObject> GetUpcomingEvent()
        {
            return await GetUpcomingEvent(DateTime.Now);
        }
        public async Task<RootEventObject> GetUpcomingEvent(DateTime date)
        {
            IList<RootEventObject> upcomingEvents = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
                    var data = await client.GetAsync(string.Concat(BASE_URL, EVENTS_URL));
                    var jsonResponse = await data.Content.ReadAsStringAsync();

                    if (jsonResponse != null)
                    {

                        upcomingEvents = JsonConvert.DeserializeObject<IList<RootEventObject>>(jsonResponse);
                    }

                    return upcomingEvents.FirstOrDefault(x => (DateTimeOffset.FromUnixTimeMilliseconds(x.time).Month == date.Month));
                }
            }

            catch (WebException exception)
            {
                throw new WebException("An error has occurred while calling GetUpcomingEvents method: " + exception.Message);
            }
        }

        public async Task<RootEventObject> GetSpecificEvent()
        {
            IList<RootEventObject> upcomingEvents = null;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);
                    var data = await client.GetAsync(string.Concat(BASE_URL, EVENTS_URL));
                    var jsonResponse = await data.Content.ReadAsStringAsync();

                    if (jsonResponse != null)
                    {

                        upcomingEvents = JsonConvert.DeserializeObject<IList<RootEventObject>>(jsonResponse);
                    }

                    return upcomingEvents.FirstOrDefault();
                }
            }

            catch (WebException exception)
            {
                throw new WebException("An error has occurred while calling GetUpcomingEvents method: " + exception.Message);
            }
        }
    }
}
