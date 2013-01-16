using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCalendar.Data
{
    [DataContract]
    public class UserData
    {
        /// <summary>The user's name, if available.</summary>
        [DataMember(Name = "displayName", EmitDefaultValue = false)]
        public string DisplayName { get; set; }

        /// <summary>The  user's email address, if available.</summary>
        [DataMember(Name = "email", EmitDefaultValue = false)]
        public string Email { get; set; }
    }
}
