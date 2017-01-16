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
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            Response response;
            IOutputSpeech innerResponse = null;
            var log = context.Logger;

            if (input.GetRequestType() == typeof(Slight.Alexa.Framework.Models.Requests.RequestTypes.ILaunchRequest))
            {
                // default launch request, let's just let them know what you can do
                log.LogLine($"Default LaunchRequest made");

                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "Welcome to the Birmingham .NET Meetup.  You can ask me about upcoming meetups and events";
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
                    var date = input.Request.Intent.Slots.Any() ? DateTime.Parse(input.Request.Intent.Slots.FirstOrDefault(x => x.Key == "Date").Value.Value) : DateTime.Now ;
                    log.LogLine($"Date understood as {date.ToString()}");

                    log.LogLine($"New request for {input.Session.User.AccessToken}");
                    var helper = new MeetupApiHelper(input.Session.User.AccessToken);
                    var upcomingEvent = await helper.GetUpcomingEvent(date);

                    log.LogLine($"Got response from Meetup{upcomingEvent}");

                    innerResponse = new PlainTextOutputSpeech();
                    (innerResponse as PlainTextOutputSpeech).Text = $"The next event is {upcomingEvent.name}.";

                  //  if(upcomingEvent)
                }


            }
            response = new Response();
            response.ShouldEndSession = true;
            response.OutputSpeech = innerResponse;
            SkillResponse skillResponse = new SkillResponse();
            skillResponse.Response = response;
            skillResponse.Version = "1.0";

            return skillResponse;
        }
    }
}
