using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ClaimEntry> ClaimEntries { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<TeaPackingLedger> TeaPackingLedgers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<FarmerLoan> FarmerLoans { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeaPackingLedger>()
                .Property(t => t.SoldPriceKg)
                .HasColumnType("decimal(18,2)");

            // Configuration for Claims
            modelBuilder.Entity<Claim>()
                .Property(c => c.Status)
                .IsRequired();

            modelBuilder.Entity<Claim>()
                .Property(c => c.Season)
                .IsRequired();

            modelBuilder.Entity<Claim>()
                .Property(c => c.GardenMark)
                .IsRequired();

            modelBuilder.Entity<Claim>()
                .Property(c => c.Invoice)
                .IsRequired();
        }
    }
}