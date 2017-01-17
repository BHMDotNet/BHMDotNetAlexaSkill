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
            var helper = new MeetupApiHelper("391da51798081577500667a1e0e7b66f");
            var UpcomingEvent = await helper.GetUpcomingEvent(DateTime.Parse("2017-06"));
        }
    }

}
