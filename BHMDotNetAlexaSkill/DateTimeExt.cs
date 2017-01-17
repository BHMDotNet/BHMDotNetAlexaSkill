using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BHMDotNetAlexaSkill
{
    /// <summary>
    /// Some helper methods I created to deal with Unix time from Meetup because...yeah.
    /// Honestly, why doesn't the DateTime class handle this nativley. This is 2017, we should not be dealing with problems from 1970 anymore.
    /// </summary>
    public static class DateTimeExt
    {
        public static DateTime FromUnixTime(this long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
    }
}
