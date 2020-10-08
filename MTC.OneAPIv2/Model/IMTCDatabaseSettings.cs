using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTC.OneAPIv2.Model
{
    public interface IMTCDatabaseSettings
    {
        string MTCLocationsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class MTCDatabaseSettings : IMTCDatabaseSettings
    {
        public string MTCLocationsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
