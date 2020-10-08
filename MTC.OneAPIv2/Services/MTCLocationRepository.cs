using Microsoft.AspNetCore.Razor.Language;
using MTC.OneAPIv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTC.OneAPIv2.Services
{
    public interface IMTCLocationRepository
    {
        IEnumerable<MTCLocation> All { get; }
        MTCLocation Find(string id);
    }

    public class MTCLocationRepository : IMTCLocationRepository
    {
        private List<MTCLocation> _locations;

        public MTCLocationRepository()
        {
            InitializeMockData();
        }

        private void InitializeMockData()
        {
            _locations = new List<MTCLocation>()
            {
                new MTCLocation()
                {
                    Id="1",
                    City = "Moscow",
                    Region="CEE",
                    Country="Russia",
                    DCAlias="oleg",
                    BCAlias="vss",
                    DirectorAlias="Oleg",
                    Address="lesnaya 9",
                    Phone="1231213213212"
                }
            };

        }

        public IEnumerable<MTCLocation> All {  get { return _locations;  } }

        public MTCLocation Find(string id)
        {
            return _locations.FirstOrDefault(item => item.Id == id);
        }
    }
}
