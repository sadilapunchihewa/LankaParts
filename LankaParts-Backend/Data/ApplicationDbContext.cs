using Microsoft.EntityFrameworkCore;
using LankaParts_Backend.Models;

namespace LankaParts_Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SellerCompany> SellerCompanies { get; set; }
        public DbSet<PartCategory> PartCategories { get; set; }
        public DbSet<SparePart> SpareParts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<SellerCompany>()
                .HasIndex(c => c.UserId)
                .IsUnique();

            modelBuilder.Entity<SellerCompany>()
                .HasIndex(c => c.BusinessRegistrationNumber)
                .IsUnique();

            modelBuilder.Entity<SellerCompany>()
                .HasOne(c => c.User)
                .WithOne(u => u.SellerCompany)
                .HasForeignKey<SellerCompany>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SellerCompany>()
                .HasOne(c => c.ReviewedByUser)
                .WithMany()
                .HasForeignKey(c => c.ReviewedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PartCategory>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<PartCategory>().HasData(
                new PartCategory { Id = 1, Name = "Engine" },
                new PartCategory { Id = 2, Name = "Brakes" },
                new PartCategory { Id = 3, Name = "Suspension" },
                new PartCategory { Id = 4, Name = "Electrical" },
                new PartCategory { Id = 5, Name = "Body" },
                new PartCategory { Id = 6, Name = "Transmission" },
                new PartCategory { Id = 7, Name = "Wheels and Tyres" },
                new PartCategory { Id = 8, Name = "Accessories" }
            );

            modelBuilder.Entity<SparePart>()
                .Property(p => p.Price)
                .HasPrecision(12, 2);

            modelBuilder.Entity<SparePart>()
                .HasIndex(p => new { p.SellerCompanyId, p.PartNumber })
                .IsUnique();

            modelBuilder.Entity<SparePart>()
                .HasOne(p => p.SellerCompany)
                .WithMany(c => c.SpareParts)
                .HasForeignKey(p => p.SellerCompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SparePart>()
                .HasOne(p => p.Category)
                .WithMany(c => c.SpareParts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasIndex(i => new { i.CustomerUserId, i.SparePartId })
                .IsUnique();

            modelBuilder.Entity<CartItem>()
                .HasOne(i => i.CustomerUser)
                .WithMany()
                .HasForeignKey(i => i.CustomerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(i => i.SparePart)
                .WithMany()
                .HasForeignKey(i => i.SparePartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasIndex(o => o.OrderNumber)
                .IsUnique();

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.CustomerUser)
                .WithMany()
                .HasForeignKey(o => o.CustomerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .Property(i => i.UnitPrice)
                .HasPrecision(12, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(i => i.LineTotal)
                .HasPrecision(12, 2);

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.SparePart)
                .WithMany()
                .HasForeignKey(i => i.SparePartId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(i => i.SellerCompany)
                .WithMany()
                .HasForeignKey(i => i.SellerCompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.OrderId)
                .IsUnique();

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.PaymentNumber)
                .IsUnique();

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasIndex(r => r.OrderItemId)
                .IsUnique();

            modelBuilder.Entity<Review>()
                .HasIndex(r => new { r.SparePartId, r.CreatedAt });

            modelBuilder.Entity<Review>()
                .HasOne(r => r.CustomerUser)
                .WithMany()
                .HasForeignKey(r => r.CustomerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.SparePart)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.SparePartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.OrderItem)
                .WithOne()
                .HasForeignKey<Review>(r => r.OrderItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
