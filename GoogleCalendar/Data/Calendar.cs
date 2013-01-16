using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar.Data
{
    [DataContract]
    public class Calendar
    {
        /// <summary>Type of the resource (&quot;calendar#calendar&quot;).</summary>
        [DataMember(Name="kind",EmitDefaultValue = false)]
        public string Kind  { get; set; }

        /// <summary>ETag of the resource.</summary>
        [DataMember(Name = "etag", EmitDefaultValue = false)]
        public string Etag { get; set; }

        /// <summary>Identifier of the calendar.</summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }
        
        /// <summary>Title of the calendar.</summary>
        [DataMember(Name = "summary", EmitDefaultValue = false)]
        public string Summary { get; set; }
        
        /// <summary>The time zone of the calendar. Optional.</summary>
        [DataMember(Name = "timeZone", EmitDefaultValue = false)]
        public string TimeZone { get; set; }

        /// <summary>Description of the calendar. Optional.</summary>
        [DataMember(Name = "description", EmitDefaultValue = false)]
        public string Description { get; set; }
        
        /// <summary>Geographic location of the calendar as free-form text. Optional.</summary>
        [DataMember(Name = "location", EmitDefaultValue = false)]
        public string Location { get; set; }

    }  
}
