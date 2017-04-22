using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HRApi.Models
{
    public class HRContext : IdentityDbContext<IdentityUser>
    {
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<RegUser> RegUsers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<AutoGenHistory> History { get; set; }
    }
}

 