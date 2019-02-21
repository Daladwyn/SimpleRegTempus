using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegTempus.Models
{
    //public class RegTempusDbContext : DbContext
    public class RegTempusDbContext : IdentityDbContext<IdentityUser>
    {
        public RegTempusDbContext(DbContextOptions<RegTempusDbContext> options) : base(options)
        {

        }
        //public DbSet<IdentityUser> dentityUsers { get; set; }
        public DbSet<Registrator> Registrators { get; set; }

        public DbSet<TimeMeasurement> TimeMeasurements { get; set; }
    }
}
