using MTC.Bot.DataAccessLayer.POCO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MTC.Bot.DataAccessLayer.Mock
{
    public static class DalMockHelper
    {
        public static List<Region> Regions
           /*{
             return*/ = new List<Region> {                
                new Region()
                {
                    Id = "d39046fe-a536-4ca6-93e5-fd90fa702ddb",
                    Title = "North America"
                },
                new Region()
                {
                    Id = "4ef65dc1-3307-4593-9f61-e9c2ec7574ed",
                    Title = "LATAM"
                },
                new Region()
                {
                    Id = "b894084d-5352-444f-b4be-fa51b853254e",
                    Title = "EMEA"
                },                
                new Region()
                {
                    Id = "c85b032b-e7d7-4a32-a3b6-b8f26ad58627",
                    Title = "Asia"
                }
            };
        //} 
        

        public static List<Location> GetLocations(string region)
        {
            var items = new List<Location>();
            
            var latam = new List<Location>()
            {
                new Location { Title = "Mexico City", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                new Location { Title = "Sao Paulo", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
            };
            var northAmerica = new List<Location>()
                {
                    new Location { Title = "Ottawa", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Seattle", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Montreal", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Houston", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Toronto", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Boston", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Chicago", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Silicon Valley", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Reston", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Dallas", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Irvine", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "New York", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Atlanta", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Minneapolis", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Philadelphia", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Detroit", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "St Louis", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Calgary", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Denver", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                    new Location { Title = "Toronto Downtown", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"}
                };

            var emea = new List<Location>()
            {
                new Location { Title = "Moscow", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
                new Location { Title = "Warsaw", Id = "707fadef-e44e-ea11-a813-000d3a8c0957"},
            };

            if (region.ToUpper() == "LATAM")
            {
                return latam.OrderBy(item => item.Title).ToList<Location>(); ;
            }
            else if (region == "North America")
            {
                return northAmerica.OrderBy(item => item.Title).ToList<Location>(); ;
            }            
            items.AddRange(latam);
            items.AddRange(northAmerica);
            items.AddRange(emea);
            return items.OrderBy(item => item.Title).ToList<Location>();  
        }

        public static string GetRegionByLocation(string location)
        {
            return "North America";
        }

        public static List<EngagementType> GetEngagementTypes()
        {
            return new List<EngagementType>()
            {
                new EngagementType{ 
                    DisplayName = "Envisioning workshop", 
                    Value = "envisioning workshop",
                    Icon = "https://mtcrequestbot20200907221020.azurewebsites.net/EnvisioningWorkshop.png"
                },
                new EngagementType{ DisplayName = "Strategy briefing", 
                    Value = "strategy briefing",
                    Icon = "https://mtcrequestbot20200907221020.azurewebsites.net/StrategyBriefing.png"},
                new EngagementType{ DisplayName = "Architecture design session", 
                    Value = "architecture design session",
                    Icon = "https://mtcrequestbot20200907221020.azurewebsites.net/ads.png"},
                new EngagementType{ DisplayName = "Rapid prototype", 
                    Value = "rapid prototype",
                    Icon = "https://mtcrequestbot20200907221020.azurewebsites.net/rapidprototype.png"},
                new EngagementType{ DisplayName = "Hackathon", 
                    Value = "hackathon",
                    Icon="https://mtcrequestbot20200907221020.azurewebsites.net/hackathon.png"},
                new EngagementType{ DisplayName = "Hands-on lab", 
                    Value = "Hands-on lab",
                    Icon="https://mtcrequestbot20200907221020.azurewebsites.net/hol.png"},
            };
        }
    }
}
