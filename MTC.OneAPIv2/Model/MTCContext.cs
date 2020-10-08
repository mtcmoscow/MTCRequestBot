using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTC.OneAPIv2.Model
{
    public class MTCContext: DbContext
    {
        public MTCContext(DbContextOptions<MTCContext> options):base(options)
        {

        }

        public DbSet<MTCLocation> Locations { get; set; }
    }
}
