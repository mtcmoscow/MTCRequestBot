using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTC.OneAPIv2.Model;
using MTC.OneAPIv2.Services;

namespace MTC.OneAPIv2.Controllers
{
    //[Route("api/[controller]")]
    [Route("/")]
    [ApiController]
    public class MTCLocationsv3Controller : ControllerBase
    {
        private readonly IMTCLocationRepository _repository;

        public MTCLocationsv3Controller(IMTCLocationRepository repository) { _repository = repository;  }

        [HttpGet]
        public IActionResult List() { return Ok(_repository.All); }
    }
}
