using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar.Data
{
    [DataContract]
    public class CalendarListEntry : Calendar
    {
        /// <summary>The effective access role that the authenticated user has on the calendar. Read-only. Possible values are:  
        ///- &quot;freeBusyReader&quot; - Provides read access to free/busy information. 
        ///- &quot;reader&quot; - Provides read access to the calendar. Private events will appear to users with reader access, but event details will be hidden. 
        ///- &quot;writer&quot; - Provides read and write access to the calendar. Private events will appear to users with writer access, and event details will be visible. 
        ///- &quot;owner&quot; - Provides ownership of the calendar. This role has all of the permissions of the writer role with the additional ability to see and manipulate ACLs.</summary>
        [DataMember(Name = "accessRole", EmitDefaultValue = false)]
        public string AccessRole { get; set; }
        
        /// <summary>The color of the calendar. This is an ID referring to an entry in the &quot;calendar&quot; section of the colors definition (see the &quot;colors&quot; endpoint). Optional.</summary>
        [DataMember(Name = "colorId", EmitDefaultValue = false)]
        public string ColorId { get; set; }    

        /*
        /// <summary>The default reminders that the authenticated user has for this calendar.</summary>
        [DataMember(Name = "defaultReminders", EmitDefaultValue = false)]
        public IList<EventReminder> DefaultReminders { get; set; }
        */
        /// <summary>Whether the calendar has been hidden from the list. Optional. The default is False.</summary>
        [DataMember(Name = "hidden", EmitDefaultValue = false)]
        public System.Nullable<bool> Hidden { get; set; }

        /// <summary>Whether the calendar content shows up in the calendar UI. Optional. The default is True.</summary>
        [DataMember(Name = "selected", EmitDefaultValue = false)]
        public System.Nullable<bool> Selected { get; set; }

        /// <summary>The summary that the authenticated user has set for this calendar. Optional.</summary>
        [DataMember(Name = "summaryOverride", EmitDefaultValue = false)]
        public string SummaryOverride { get; set; }
    }
}
