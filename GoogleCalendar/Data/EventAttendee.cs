using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar.Data
{
    [DataContract]
    public class EventAttendee
    {
        /// <summary>Number of additional guests. Optional. The default is 0.</summary>
        [DataMember(Name = "additionalGuests", EmitDefaultValue = false)]
        public System.Nullable<long> AdditionalGuests { get; set; }

        /// <summary>The attendee&apos;s response comment. Optional.</summary>
        [DataMember(Name = "comment", EmitDefaultValue = false)]
        public string Comment { get; set; }

        /// <summary>The attendee&apos;s name, if available. Optional.</summary>
        [DataMember(Name = "displayName", EmitDefaultValue = false)]
        public string DisplayName { get; set; }

        /// <summary>The attendee&apos;s email address, if available. This field must be present when adding an attendee.</summary>
        [DataMember(Name = "email", EmitDefaultValue = false)]
        public string Email { get; set; }

        /// <summary>Whether this is an optional attendee. Optional. The default is False.</summary>
        [DataMember(Name = "optional", EmitDefaultValue = false)]
        public System.Nullable<bool> Optional { get; set; }

        /// <summary>Whether the attendee is the organizer of the event. Read-only. The default is False.</summary>
        [DataMember(Name = "organizer", EmitDefaultValue = false)]
        public System.Nullable<bool> Organizer { get; set; }

        /// <summary>Whether the attendee is a resource. Read-only. The default is False.</summary>
        [DataMember(Name = "resource", EmitDefaultValue = false)]
        public System.Nullable<bool> Resource { get; set; }

        /// <summary>The attendee&apos;s response status. Possible values are:  
        ///- &quot;needsAction&quot; - The attendee has not responded to the invitation. 
        ///- &quot;declined&quot; - The attendee has declined the invitation. 
        ///- &quot;tentative&quot; - The attendee has tentatively accepted the invitation. 
        ///- &quot;accepted&quot; - The attendee has accepted the invitation.</summary>
        [DataMember(Name = "responseStatus", EmitDefaultValue = false)]
        public string ResponseStatus { get; set; }

        /// <summary>Whether this entry represents the calendar on which this copy of the event appears. Read-only. The default is False.</summary>
        [DataMember(Name = "self", EmitDefaultValue = false)]
        public System.Nullable<bool> Self { get; set; }
    }
}
