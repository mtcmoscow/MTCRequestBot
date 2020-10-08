using MongoDB.Driver;
using MTC.OneAPIv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTC.OneAPIv2.Services
{
    public class MTCLocationsService
    {
        private readonly IMongoCollection<MTCLocation> _locations;

        public MTCLocationsService(IMTCDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _locations = database.GetCollection<MTCLocation>(settings.MTCLocationsCollectionName);
        }

        public List<MTCLocation> Get() => _locations.Find(item => true).ToList();

        public MTCLocation Get(string id) => _locations.Find<MTCLocation>(item => item.Id == id).FirstOrDefault();
    }
}
