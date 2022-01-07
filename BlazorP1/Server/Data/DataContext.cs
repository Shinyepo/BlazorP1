using BlazorP1.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorP1.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Battle>()
                .HasOne(x => x.Attacker)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Battle>()
                .HasOne(x=>x.Opponent)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Battle>()
                .HasOne(x=>x.Winner)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserUnit> UserUnits { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<UnitBuilding> UnitBuildings { get; set; }
        public DbSet<ProductionBuilding> ProductionBuildings { get; set; }
        public DbSet<UtilityBuilding> UtilityBuildings { get; set; }
        public DbSet<UserBuilding> UserBuildings { get; set; }
        public DbSet<UserBuildingTask> UserBuildingTasks { get; set; }
        public DbSet<MissionLevel> MissionLevels { get; set; }
        public DbSet<MissionUnits> MissionUnits { get; set; }
    }
}
