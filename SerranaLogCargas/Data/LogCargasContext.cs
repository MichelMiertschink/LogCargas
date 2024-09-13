using Microsoft.EntityFrameworkCore;
using LogCargas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace LogCargas.Data
{
    public class LogCargasContext : IdentityDbContext
    {
        public LogCargasContext (DbContextOptions<LogCargasContext> options)
            :base(options) 
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }
        public DbSet<State> States { get; set; } = default!;
        public DbSet<City> Cities {  get; set; } = default!;
        public DbSet<Customer> Customers { get; set; } = default!;
        public DbSet<Driver> Driver { get; set; } = default!;
        public DbSet<Vehicle> Vehicles { get; set; } = default!;
        public DbSet<LoadScheduling> LoadScheduling { get; set; } = default!;
        public DbSet<RedeFrota> RedeFrota { get; } = default!;
        /*public DbSet<IdentityUser> IdentityUser { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }*/
    }
}

