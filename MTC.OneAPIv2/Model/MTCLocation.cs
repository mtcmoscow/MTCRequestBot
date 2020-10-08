using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTC.OneAPIv2.Model
{
    //    {
    //    "id": "1",
    //    "mtclocation": "Moscow",
    //    "country": "Russia",
    //    "region": "CEE",
    //    "dalias": "oleg",
    //    "bcalias": "sez",
    //    "dcmalias": "gurgig",
    //    "address": "White Gardens Business Center 9 Lesnaya St. Moscow  125047 Russia​",
    //    "phone": "007 495 789 84 44",
    //    "mtcimage": ""
    //}

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class MTCLocation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SystemId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [BsonElement("city")]
        [JsonProperty("mtclocation")]
        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        [JsonProperty("dalias")]
        public string DirectorAlias { get; set; }
        public string BCAlias { get; set; }
        public string DCAlias { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
    }
}
