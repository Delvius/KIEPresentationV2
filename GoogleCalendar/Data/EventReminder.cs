using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar.Data
{
    [DataContract]
    public class EventReminder
    {
        /// <summary>The method used by this reminder. Possible values are:  
        ///- &quot;email&quot; - Reminders are sent via email. 
        ///- &quot;sms&quot; - Reminders are sent via SMS. 
        ///- &quot;popup&quot; - Reminders are sent via a UI popup.</summary>
        [DataMember(Name = "method", EmitDefaultValue = false)]
        public string Method { get; set; }

        /// <summary>Number of minutes before the start of the event when the reminder should trigger.</summary>
        [DataMember(Name = "minutes", EmitDefaultValue = false)]
        public System.Nullable<long> Minutes { get; set; }
    }
}
