using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMaker.Models {
    public class User {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string EmailAddress { get; set; }
        public DateTime LastSignInDateTime { get; set; }
        public string RegistrationStatus { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public string ApprovedByAccount { get; set; }
        public Phone[] Phones { get; set; }
        public UserToLocation[] LocationIds { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class UserToLocation {
        public string LocationId { get; set; }
        public UserRole[] Roles { get; set; }
    }

    public class UserRole {
        public string Role { get; set; }
    }
    public class Phone {
        public string PhoneType { get; set; }
        public string PhoneNumber { get; set; }
        public bool isPreferred { get; set; }
    }
}
