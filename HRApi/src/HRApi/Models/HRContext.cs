using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HRApi.Models
{
    public class HRContext : IdentityDbContext<RegUser>
    {
        public HRContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<RegUser> RegUsers { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<AutoGenHistory> History { get; set; }

    }
}

