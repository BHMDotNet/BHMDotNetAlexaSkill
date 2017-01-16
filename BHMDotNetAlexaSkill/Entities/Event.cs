using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BHMDotNetAlexaSkill.Entities
{

    public class Venue
    {
        public int id { get; set; }
        public string name { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public bool repinned { get; set; }
        public string address_1 { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string localized_country_name { get; set; }
        public string zip { get; set; }
        public string state { get; set; }
    }

    public class Group
    {
        public long created { get; set; }
        public string name { get; set; }
        public int id { get; set; }
        public string join_mode { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public string urlname { get; set; }
        public string who { get; set; }
    }

    public class Event
    {
        public long created { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public long time { get; set; }
        public long updated { get; set; }
        public int utc_offset { get; set; }
        public int waitlist_count { get; set; }
        public int yes_rsvp_count { get; set; }
        public Venue venue { get; set; }
        public Group group { get; set; }
        public string link { get; set; }
        public int manual_attendance_count { get; set; }
        public string description { get; set; }
        public string visibility { get; set; }
    }

    public class RootEventObject
    {
        public Event Event { get; set; }
    }

}
