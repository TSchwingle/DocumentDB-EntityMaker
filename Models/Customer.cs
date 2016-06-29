using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMaker.Models {
    public class Customer {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string CompanyCode { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public Location[] Locations { get; set; }
        public UserDefinedField[] UserDefinedFields { get; set; }
        public Note[] Notes { get; set; }
        public HeadQuarter HeadQuarters { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class HeadQuarter {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Location {
        public string LocationId { get; set; }
        public string LocationName { get; set; }        
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool IsActive { get; set; }

    }

    public class UserDefinedField {
        public string EntityName { get; set; }
        public string EntityType { get; set; }
        public string EntityValue { get; set; }
    }
    public class Note {
        public string NoteType { get; set; }
        public string Text { get; set; }
        public DateTime Added { get; set; }
    }
}

