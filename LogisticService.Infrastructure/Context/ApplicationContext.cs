using LogisticService.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace LogisticService.Infrastructure.Context;

public class ApplicationContext : DbContext
    {
        public DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();
        public DbSet<ShippingEntity> Shippings => Set<ShippingEntity>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ShippingEntity>()
                .HasOne(s => s.Vehicle)
                .WithMany(v => v.Shippings)
                .HasForeignKey(s => s.VehicleId);
        }
    }