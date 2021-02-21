using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MTCWhoBotPrototype.Helpers
{
    public class OneMTCApiHelper
    {
        public static async Task<People> GetAll()
        {
            var contentsJson = new People();
            using (var client = new HttpClient())
            {
                //string baseUrl = "";
                //client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "aa43422280b44eb490b8826112951b2c");
                client.DefaultRequestHeaders.Add("Ocp-Apim-Trace", "true");

                //string filter = "ownerId eq " + "/users/" + user.Id; //this is likely where the issue is

                var response = await client.GetAsync("https://mibonapimdev.azure-api.net/mtcdemo/manual/paths/invoke");
                var contents = await response.Content.ReadAsStringAsync();
                contentsJson = JsonConvert.DeserializeObject<People>(contents);
            };
            return contentsJson;
        }
    }

    public class People
    {
        [JsonProperty(PropertyName = "users")]
        public List<MTCUser> Users { get; set; }
    }

    public class MTCUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "alias")]
        public string Alias { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "mtc")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "manager")]
        public string Manager { get; set; }

        [JsonProperty(PropertyName = "qualifier")]
        public string Qualifier { get; set; }

        [JsonProperty(PropertyName = "industry")]
        public string Industry { get; set; }

        [JsonProperty(PropertyName = "keyProgrammes")]
        public string KeyProgrammes { get; set; }      
        
        [JsonProperty(PropertyName = "skills")]
        public string Skills { get; set; }
    }
}
