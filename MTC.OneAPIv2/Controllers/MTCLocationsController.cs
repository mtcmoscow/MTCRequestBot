using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTC.OneAPIv2.Model;

namespace MTC.OneAPIv2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MTCLocationsController : ControllerBase
    {
        private readonly MTCContext _context;

        public MTCLocationsController(MTCContext context)
        {
            _context = context;
        }

        // GET: api/MTCLocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MTCLocation>>> GetLocations()
        {
            return await _context.Locations.ToListAsync();
        }

        // GET: api/MTCLocations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MTCLocation>> GetMTCLocation(string id)
        {
            var mTCLocation = await _context.Locations.FindAsync(id);

            if (mTCLocation == null)
            {
                return NotFound();
            }

            return mTCLocation;
        }

        // PUT: api/MTCLocations/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMTCLocation(string id, MTCLocation mTCLocation)
        {
            if (id != mTCLocation.Id)
            {
                return BadRequest();
            }

            _context.Entry(mTCLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MTCLocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MTCLocations
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MTCLocation>> PostMTCLocation(MTCLocation mTCLocation)
        {
            _context.Locations.Add(mTCLocation);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MTCLocationExists(mTCLocation.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMTCLocation", new { id = mTCLocation.Id }, mTCLocation);
        }

        // DELETE: api/MTCLocations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MTCLocation>> DeleteMTCLocation(string id)
        {
            var mTCLocation = await _context.Locations.FindAsync(id);
            if (mTCLocation == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(mTCLocation);
            await _context.SaveChangesAsync();

            return mTCLocation;
        }

        private bool MTCLocationExists(string id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
    }
}
