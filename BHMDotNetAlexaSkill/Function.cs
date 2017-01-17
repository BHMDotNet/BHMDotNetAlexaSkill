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
                (innerResponse as PlainTextOutputSpeech).Text = "Welcome to the Birmingham dot net Meetup.  You can say things like 'When is the next meetup?' or 'What meetups are in June?'";
            }
            else if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.IIntentRequest))
            {
                // intent request, process the intent
                log.LogLine($"Intent Requested {input.Request.Intent.Name}");


                if ("Test".Equals(input.Request.Intent.Name))
                {
                    innerResponse = new PlainTextOutputSpeech();
                    (innerResponse as PlainTextOutputSpeech).Text = "All systems online and nominal.";
                }
                if ("Introduction".Equals(input.Request.Intent.Name))
                {
                    innerResponse = new PlainTextOutputSpeech();
                    (innerResponse as PlainTextOutputSpeech).Text = "Good evening developers. My name is Alexa, and I'm proud to introduce tonight's speaker...well, the person speaking. Not an actual speaker, that's my job. Anyway, tonight's speaker is Blake Helms. He is a Software Development Manager at EBSCO Industries. There he is responsible for several core business applications and has been a driver for software craftsmanship and creating a culture that promotes mentorship and continuous improvement. He is one of the cofounders of the Birmingham dot net meetup. Blake is incredibly passionate about technology in all areas from writing code for work, to audio/video production for his church to automating his home. Now without further ado here is Blake Helms! ";
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
