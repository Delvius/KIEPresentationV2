using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar.Data
{
    [DataContract]
    public class Events
    {
        /// <summary>The user&apos;s access role for this calendar. Read-only. Possible values are:  
        ///- &quot;none&quot; - The user has no access. 
        ///- &quot;freeBusyReader&quot; - The user has read access to free/busy information. 
        ///- &quot;reader&quot; - The user has read access to the calendar. Private events will appear to users with reader access, but event details will be hidden. 
        ///- &quot;writer&quot; - The user has read and write access to the calendar. Private events will appear to users with writer access, and event details will be visible. 
        ///- &quot;owner&quot; - The user has ownership of the calendar. This role has all of the permissions of the writer role with the additional ability to see and manipulate ACLs.</summary>
        [DataMember(Name = "accessRole", EmitDefaultValue = false)]
        public string AccessRole { get; set; }

        /// <summary>The default reminders on the calendar for the authenticated user. These reminders apply to all events on this calendar that do not explicitly override them (i.e. do not have &apos;reminders.useDefault&apos; set to &apos;true&apos;).</summary>
        [DataMember(Name = "defaultReminders", EmitDefaultValue = false)]
        public IList<EventReminder> DefaultReminders { get; set; }

        /// <summary>Description of the calendar. Read-only.</summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>ETag of the collection.</summary>
        [DataMember(Name = "etag", EmitDefaultValue = false)]
        public string Etag { get; set; }

        /// <summary>List of events on the calendar.</summary>
        [DataMember(Name = "items", EmitDefaultValue = false)]
        public IList<Event> Items { get; set; }

        /// <summary>Type of the collection (&quot;calendar#events&quot;).</summary>
        [DataMember(Name = "kind", EmitDefaultValue = false)]
        public string Kind { get; set; }

        /// <summary>Token used to access the next page of this result. Omitted if no further results are available.</summary>
        [DataMember(Name = "nextPageToken", EmitDefaultValue = false)]
        public string NextPageToken { get; set; }

        /// <summary>Title of the calendar. Read-only.</summary>
        [DataMember(Name = "summary", EmitDefaultValue = false)]
        public string Summary { get; set; }

        /// <summary>The time zone of the calendar. Read-only.</summary>
        [DataMember(Name = "timeZone", EmitDefaultValue = false)]
        public string TimeZone { get; set; }

        /// <summary>Last modification time of the calendar (as a RFC 3339 timestamp). Read-only.</summary>
        [DataMember(Name = "updated", EmitDefaultValue = false)]
        public string Updated { get; set; }
        
    }
}
