using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRApi.Models
{
    public class HRInfoContext : DbContext
    {
        public HRInfoContext(DbContextOptions<HRInfoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<RegUser> RegUsers { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<AutoGenHistory> History { get; set; }
    }
}
}
