using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMaker.Models {
    public class AssetHistory {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string CustomerId { get; set; }


        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
    }
}