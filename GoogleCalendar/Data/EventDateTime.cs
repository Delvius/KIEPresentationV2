using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar.Data
{
    [DataContract]
    public class EventDateTime
    {
        /// <summary>The date, in the format &quot;yyyy-mm-dd&quot;, if this is an all-day event.</summary>
        [DataMember(Name = "date", EmitDefaultValue = false)]
        public string Date { get; set; }

        /// <summary>The time, as a combined date-time value (formatted according to RFC 3339). A time zone offset is required unless a time zone is explicitly specified in &apos;timeZone&apos;.</summary>
        [DataMember(Name = "dateTime", EmitDefaultValue = false)]
        public string DateTime { get; set; }

        /// <summary>The name of the time zone in which the time is specified (e.g. &quot;Europe/Zurich&quot;). Optional. The default is the time zone of the calendar.</summary>
        [DataMember(Name = "timeZone", EmitDefaultValue = false)]
        public string TimeZone { get; set; }
    }
}
