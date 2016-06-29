using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMaker.Models {
    public class Device {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string DeviceType { get; set; }
        public string DeviceName { get; set; }
        public string TagType { get; set; }
        public string DeviceId { get; set; }
        public string AssetId { get; set; }   
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }

    }
}