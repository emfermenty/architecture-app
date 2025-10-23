using architectureProject.Models;
using architectureProject.Models.enums;
using Microsoft.EntityFrameworkCore;

namespace architectureProject.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Shipping> Shippings => Set<Shipping>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Vehicle - Table Per Hierarchy (TPH)
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);
        
                entity.HasDiscriminator<VehicleType>("VehicleType")
                    .HasValue<Truck>(VehicleType.Truck)
                    .HasValue<CargoShip>(VehicleType.CargoShip)
                    .HasValue<FreightTrain>(VehicleType.FreightTrain)
                    .HasValue<CargoPlain>(VehicleType.CargoPlain);
                
                
                // Общие свойства для всех Vehicle
                entity.Property(v => v.Model).IsRequired().HasMaxLength(50);
                entity.Property(v => v.MaxWeight).HasColumnType("double precision");
                entity.Property(v => v.MaxVolume).HasColumnType("double precision");
                entity.Property(v => v.Speed).HasColumnType("double precision");
                entity.Property(v => v.FuelConsumption).HasColumnType("double precision");
        
                // КОНФИГУРАЦИЯ ОТНОШЕНИЯ С SHIPPING
                entity.HasMany(v => v.Shippings)
                    .WithOne(s => s.Vehicle)
                    .HasForeignKey(s => s.VehicleId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            
            modelBuilder.Entity<Shipping>(entity =>
            {
                entity.HasKey(s => s.Id);
        
                entity.HasDiscriminator<ShippingType>("ShippingType")
                    .HasValue<TruckShipping>(ShippingType.Truck)
                    .HasValue<SeaShipping>(ShippingType.Sea)
                    .HasValue<TrainShipping>(ShippingType.Train)
                    .HasValue<AirShipping>(ShippingType.Air);

                entity.HasIndex(s => s.TrackingNumber).IsUnique();
        
                // Общие свойства для всех Shipping
                entity.Property(s => s.TrackingNumber).IsRequired().HasMaxLength(20);
                entity.Property(s => s.Distance).HasColumnType("double precision");
                entity.Property(s => s.Weight).HasColumnType("double precision");
                entity.Property(s => s.Volume).HasColumnType("double precision");
                entity.Property(s => s.Cost).HasColumnType("integer");
                entity.Property(s => s.Duration).HasColumnType("interval");
            });
        }
    }
}