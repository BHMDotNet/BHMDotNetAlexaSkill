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
            var helper = new MeetupApiHelper("Test");
            var UpcomingEvent = helper.GetUpcomingEvent();
        }
    }
}
