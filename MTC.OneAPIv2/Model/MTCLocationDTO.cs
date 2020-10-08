using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTC.OneAPIv2.Model
{
    /// <summary>
    /// MTCLocation without PII
    /// </summary>
    public class MTCLocationDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("mtclocation")]
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
    }
}
