using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMaker.Models {
    public class Asset {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string LocationId { get; set; }
        public string TagId { get; set; }
        public string TagType { get; set; }
        public string AssetId { get; set; }
        public string AssetName { get; set; }  
        public int HealthScore { get; set; }
        public string BatteryStatus { get; set; }   
        public decimal RSSI { get; set; }
        public decimal Temperature { get; set; }
        public string[] mZones { get; set; }
        public string AssetStatus { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
    }
}
