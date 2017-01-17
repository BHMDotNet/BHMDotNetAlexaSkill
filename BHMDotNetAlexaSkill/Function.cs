using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization;
using Slight.Alexa.Framework.Models.Requests;
using Slight.Alexa.Framework.Models.Responses;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializerAttribute(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace BHMDotNetAlexaSkill
{
    public class Function
    {
        private bool _endSession = true;
      
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            Response response;
            IOutputSpeech innerResponse = null;
            var log = context.Logger;

            if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.ILaunchRequest))
            {
                // default launch request, let's just let them know what you can do
                log.LogLine($"Default LaunchRequest made");
                _endSession = false;
                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "Welcome to the Birmingham .NET Meetup.  You can say things like 'When is the next meetup?' or 'What meetups are in June?'";
            }
            else if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.IIntentRequest))
            {
                // intent request, process the intent
                log.LogLine($"Intent Requested {input.Request.Intent.Name}");


                if ("Test".Equals(input.Request.Intent.Name))
                {
                    innerResponse = new PlainTextOutputSpeech();
                    (innerResponse as PlainTextOutputSpeech).Text = "Atomic batteries to power. Turbines to speed. All systems online and nominal.";
                }
                else if ("UpcomingEvent".Equals(input.Request.Intent.Name))
                {
                    log.LogLine($"New request for {input.Session.User.AccessToken}");

                    var date = input.Request.Intent.Slots.Any() && 
                        input.Request.Intent.Slots.FirstOrDefault(x => x.Key == "Date").Value != null && 
                       !string.IsNullOrWhiteSpace(input.Request.Intent.Slots.FirstOrDefault(x => x.Key == "Date").Value.Value) ? 
                        DateTime.Parse(input.Request.Intent.Slots.FirstOrDefault(x => x.Key.ToLower() == "date").Value.Value) : 
                        DateTime.Now ;

                    log.LogLine($"Date understood as {date.ToString()}");
                    
                    var helper = new MeetupApiHelper(input.Session.User.AccessToken);
                    var upcomingEvent = await helper.GetUpcomingEvent(date);

                    log.LogLine($"Got response from Meetup{upcomingEvent}");

                    var text = "No events for for that date. I'll let Robb and Blake know they need to plan better.";
                    if(upcomingEvent != null)
                    {
                        text = $"{upcomingEvent.name}. On {DateTimeOffset.FromUnixTimeMilliseconds(upcomingEvent.time).ToString("d")}";
                    }

                    innerResponse = new PlainTextOutputSpeech();
                    
                    (innerResponse as PlainTextOutputSpeech).Text = text;
                   
                }


            }
            response = new Response();
            response.ShouldEndSession = _endSession;
            response.OutputSpeech = innerResponse;
            SkillResponse skillResponse = new SkillResponse();
            skillResponse.Response = response;
            skillResponse.Version = "1.0";

            return skillResponse;
        }
    }
}
