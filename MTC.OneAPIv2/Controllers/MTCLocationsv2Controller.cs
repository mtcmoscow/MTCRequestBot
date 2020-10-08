using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using MTC.OneAPIv2.Model;
using MTC.OneAPIv2.Services;

namespace MTC.OneAPIv2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MTCLocationsv2Controller : ControllerBase
    {
        private readonly MTCLocationsService _locationService;

        public MTCLocationsv2Controller(MTCLocationsService locService)
        {
            _locationService = locService;
        }

        [HttpGet]
        public ActionResult<List<MTCLocation>> Get() => _locationService.Get();

        [HttpGet("{id:length(24)}", Name ="GetLocation")]
        public ActionResult<MTCLocation> Get(string id)
        {
            var item = _locationService.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }
    }
}
