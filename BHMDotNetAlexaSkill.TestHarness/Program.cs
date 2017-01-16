using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BHMDotNetAlexaSkill.TestHarness
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            var helper = new MeetupApiHelper("Test");
            var UpcomingEvent = await helper.GetUpcomingEvent();
        }
    }

}
