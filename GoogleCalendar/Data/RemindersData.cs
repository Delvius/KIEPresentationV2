using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar.Data
{
    [DataContract]
    public class RemindersData
    {
        /// <summary>If the event doesn&apos;t use the default reminders, this lists the reminders specific to the event, or, if not set, indicates that no reminders are set for this event.</summary>
        [DataMember(Name = "overrides", EmitDefaultValue = false)]
        public IList<EventReminder> Overrides { get; set; }

        /// <summary>Whether the default reminders of the calendar apply to the event.</summary>
        [DataMember(Name = "useDefault", EmitDefaultValue = false)]
        public System.Nullable<bool> UseDefault { get; set; }
    }
}
