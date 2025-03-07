using Microsoft.EntityFrameworkCore;
using HotelManagementSystem.Core.Models;

namespace HotelManagementSystem.Data.Context
{
    public class HotelDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomStatus> RoomStatuses { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
        {
            // This ensures thread safety - prevents entities from being tracked for long periods
            // which can lead to concurrency issues
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Additional configuration if needed
            base.OnConfiguring(optionsBuilder);
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Room>()
                .HasOne(r => r.RoomTypeDetails)
                .WithMany(rt => rt.Rooms)
                .HasForeignKey(r => r.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RoomStatus>()
                .HasOne(rs => rs.Room)
                .WithMany(r => r.StatusHistory)
                .HasForeignKey(rs => rs.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Guest)
                .WithMany(g => g.Reservations)
                .HasForeignKey(r => r.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure indexes
            modelBuilder.Entity<Room>()
                .HasIndex(r => r.RoomNumber)
                .IsUnique();

            modelBuilder.Entity<Guest>()
                .HasIndex(g => g.Email)
                .IsUnique();

            modelBuilder.Entity<Reservation>()
                .HasIndex(r => r.ReservationNumber)
                .IsUnique();
        }
    }
}