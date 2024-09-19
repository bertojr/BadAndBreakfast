using Microsoft.EntityFrameworkCore;
namespace back_end.DataModels
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AdditionalService> AdditionalServices { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomImage> RoomImages { get; set; }
        public DbSet<StripeWebHook> StripeWebHooks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // configurazione per User
            modelBuilder.Entity<User>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                "UsersRoles",
                j => j
                    .HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleID"),
                j => j
                    .HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserID"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .OnDelete(DeleteBehavior.Cascade);

            // configurazione per Room
            modelBuilder.Entity<Room>()
                .HasMany(r => r.RoomImages)
                .WithOne(i => i.Room)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Room>()
                .HasMany(r => r.Amenities)
                .WithMany(a => a.Rooms)
                .UsingEntity<Dictionary<string, object>>(
                "RoomsAmenities",
                j => j
                    .HasOne<Amenity>()
                    .WithMany()
                    .HasForeignKey("AmenityID"),
                j => j
                    .HasOne<Room>()
                    .WithMany()
                    .HasForeignKey("RoomID"));

            // configurazione per review
            modelBuilder.Entity<Review>()
                .Property(r => r.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Review>()
                .Property(r => r.UpdatedDate)
                .HasDefaultValueSql("GETDATE()");

            // configurazione per booking
            modelBuilder.Entity<Booking>()
                .HasMany(b => b.Rooms)
                .WithMany(r => r.Bookings)
                .UsingEntity<Dictionary<string, object>>(
                "BookingsRooms",
                j => j
                    .HasOne<Room>()
                    .WithMany()
                    .HasForeignKey("RoomID"),
                j => j
                    .HasOne<Booking>()
                    .WithMany()
                    .HasForeignKey("BookingID"));

            modelBuilder.Entity<Booking>()
                .HasMany(b => b.AdditionalServices)
                .WithMany(a => a.Bookings)
                .UsingEntity<Dictionary<string, object>>(
                "BookingsServices",
                j => j
                    .HasOne<AdditionalService>()
                    .WithMany()
                    .HasForeignKey("ServiceID"),
                j => j
                    .HasOne<Booking>()
                    .WithMany()
                    .HasForeignKey("BookingID"));

            // configurazione per payment
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

