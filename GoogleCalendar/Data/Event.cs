using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar.Data
{
    [DataContract]
    public class Event
    {
        /// <summary>Type of the resource (&quot;calendar#event&quot;).</summary>
        [DataMember(Name = "kind", EmitDefaultValue = false)]
        public string Kind { get; set; }

        /// <summary>Identifier of the event.</summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>Status of the event. Optional. Possible values are:  
        ///- &quot;confirmed&quot; - The event is confirmed. This is the default status. 
        ///- &quot;tentative&quot; - The event is tentatively confirmed. 
        ///- &quot;cancelled&quot; - The event is cancelled.</summary>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        public string Status { get; set; }

        /// <summary>An absolute link to this event in the Google Calendar Web UI. Read-only.</summary>
        [DataMember(Name = "htmlLink", EmitDefaultValue = false)]
        public string HtmlLink { get; set; }

        /// <summary>Creation time of the event (as a RFC 3339 timestamp). Read-only.</summary>
        [DataMember(Name = "created", EmitDefaultValue = false)]
        public string Created { get; set; }

        /// <summary>Last modification time of the event (as a RFC 3339 timestamp). Read-only.</summary>
        [DataMember(Name = "updated", EmitDefaultValue = false)]
        public string Updated { get; set; }

        /// <summary>Title of the event.</summary>
        [DataMember(Name = "summary", EmitDefaultValue = false)]
        public string Summary { get; set; }

        /// <summary>Description of the event.</summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }

        /// <summary>Geographic location of the event as free-form text. Optional.</summary>
        [DataMember(Name = "location", EmitDefaultValue = false)]
        public string Location { get; set; }

        /// <summary>The creator of the event. Read-only.</summary>
        [DataMember(Name = "creator", EmitDefaultValue = false)]
        public UserData Creator { get; set; }

        /// <summary>The organizer of the event. If the organizer is also an attendee, this is indicated with a separate entry in &apos;attendees&apos; with the &apos;organizer&apos; field set to True.</summary>
        [DataMember(Name = "organizer", EmitDefaultValue = false)]
        public UserData Organizer { get; set; }

        [DataMember(Name = "start", EmitDefaultValue = false)]
        public EventDateTime Start { get; set; }

        [DataMember(Name = "end", EmitDefaultValue = false)]
        public EventDateTime End { get; set; }

        /// <summary>Event ID in the iCalendar format.</summary>
        [DataMember(Name = "iCalUID", EmitDefaultValue = false)]
        public string ICalUID { get; set; }

        /// <summary>Sequence number as per iCalendar.</summary>
        [DataMember(Name = "sequence", EmitDefaultValue = false)]
        public System.Nullable<long> Sequence { get; set; }

        /// <summary>The attendees of the event.</summary>
        [DataMember(Name = "attendees", EmitDefaultValue = false)]
        public IList<EventAttendee> Attendees { get; set; }

        /// <summary>Whether attendees other than the organizer can invite others to the event. Optional. The default is False.</summary>
        [DataMember(Name = "guestsCanInviteOthers", EmitDefaultValue = false)]
        public System.Nullable<bool> GuestsCanInviteOthers { get; set; }

        /// <summary>Whether attendees other than the organizer can see who the event&apos;s attendees are. Optional. The default is False.</summary>
        [DataMember(Name = "guestsCanSeeOtherGuests", EmitDefaultValue = false)]
        public System.Nullable<bool> GuestsCanSeeOtherGuests { get; set; }

        /// <summary>Information about the event&apos;s reminders for the authenticated user.</summary>
        [DataMember(Name = "reminders", EmitDefaultValue = false)]
        public RemindersData Reminders { get; set; }
    }
}
