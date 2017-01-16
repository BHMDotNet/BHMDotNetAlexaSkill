using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHMDotNetAlexaSkill.TestHarness
{
    class Program
    {
        const string APIKEY = "";
        static void Main(string[] args)
        {

            var helper = new MeetupApiHelper(APIKEY);
            var upcomingEvent = helper.GetUpcomingEvent();
        }
    }
}
