using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar.Data
{
    [DataContract]
    public class CalendarList
    {
        /// <summary>Type of the collection (&quot;calendar#calendarList&quot;).</summary>
        [DataMember(Name = "kind", EmitDefaultValue = false)]
        public string Kind { get; set; }

        /// <summary>ETag of the collection.</summary>
        [DataMember(Name = "etag", EmitDefaultValue = false)]
        public string Etag { get; set; }

        /// <summary>Token used to access the next page of this result.</summary>
        [DataMember(Name = "nextPageToken", EmitDefaultValue = false)]
        public string NextPageToken { get; set; }

        /// <summary>Calendars that are present on the user&apos;s calendar list.</summary>
        [DataMember(Name = "items", EmitDefaultValue = false)]
        public IList<CalendarListEntry> Items { get; set; }

    }
}
