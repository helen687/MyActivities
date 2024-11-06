using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyActivities.Core;

namespace MyActivities.DB
{
    public class ActivitiesDBContext : DbContext
    {
        public ActivitiesDBContext(DbContextOptions<ActivitiesDBContext> options) : base(options)
        {
        }

        public DbSet<Activity> Activities { get; set; }
        public DbSet<DayActivity> DayActivities { get; set; }

        public DbSet<GymActivity> GymActivities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity => {
                entity.HasKey(a => a.Id);
            });
            modelBuilder.Entity<DayActivity>(entity => {
                entity.HasKey(da => da.Id);
            });
            modelBuilder.Entity<GymActivity>(entity => {
                entity.HasKey(ga => ga.Id);
            });
        }
    }
}
