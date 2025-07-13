using Microsoft.EntityFrameworkCore;
using paymentManager.Models;

namespace paymentManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets from the first context
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<GreenLeafData> GreenLeafData { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Advance> Advances { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<Incentive> Incentives { get; set; }
        public DbSet<PaymentHistory> PaymentHistory { get; set; }

        // DbSets from the second context
        public DbSet<DenaturedTea> DenaturedTeas { get; set; }
        public DbSet<TeaReturn> TeaReturns { get; set; }
        public DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and entities from the first context
            modelBuilder.Entity<GreenLeafData>()
                .HasOne(g => g.Supplier)
                .WithMany(s => s.GreenLeafData)
                .HasForeignKey(g => g.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.Property(e => e.LeafWeight).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Rate).HasColumnType("decimal(18,2)");
                entity.Property(e => e.GrossAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.AdvanceDeduction).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.DebtDeduction).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.IncentiveAddition).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.NetAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedBy).HasMaxLength(100);
                entity.Property(e => e.PaymentDate).IsRequired();
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(p => p.Supplier)
                    .WithMany(s => s.Payments)
                    .HasForeignKey(p => p.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Advance>(entity =>
            {
                entity.HasKey(e => e.AdvanceId);

                entity.Property(e => e.AdvanceAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.RecoveredAmount).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.BalanceAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.AdvanceType).HasMaxLength(50);
                entity.Property(e => e.Purpose).HasMaxLength(200);

                entity.HasOne(a => a.Supplier)
                    .WithMany(s => s.Advances)
                    .HasForeignKey(a => a.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Debt>(entity =>
            {
                entity.HasKey(e => e.DebtId);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DeductionsMade).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.BalanceAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DeductionPercentage).HasColumnType("decimal(5,2)").HasDefaultValue(20);
                entity.Property(e => e.DebtType).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(200);
                entity.Property(e => e.Status).HasMaxLength(20).HasDefaultValue("Active");

                entity.HasOne(d => d.Supplier)
                    .WithMany(s => s.Debts)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Incentive>(entity =>
            {
                entity.HasKey(e => e.IncentiveId);

                entity.Property(e => e.QualityBonus).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.LoyaltyBonus).HasColumnType("decimal(18,2)").HasDefaultValue(0);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Month).HasMaxLength(7);

                entity.HasOne(i => i.Supplier)
                    .WithMany(s => s.Incentives)
                    .HasForeignKey(i => i.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PaymentHistory>(entity =>
            {
                entity.HasKey(e => e.HistoryId);

                entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ActionBy).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Details).HasMaxLength(500);
                entity.Property(e => e.ActionDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(h => h.Payment)
                    .WithMany(p => p.PaymentHistories)
                    .HasForeignKey(h => h.PaymentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.SupplierId);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Contact).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Area).HasMaxLength(200);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.JoinDate).HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<GreenLeafData>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Weight).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.SupplierId).IsRequired();
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");

                entity.HasOne(g => g.Supplier)
                    .WithMany(s => s.GreenLeafData)
                    .HasForeignKey(g => g.SupplierId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Add indexes for better performance
            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.PaymentDate)
                .HasDatabaseName("IX_Payment_PaymentDate");

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.SupplierId)
                .HasDatabaseName("IX_Payment_SupplierId");

            modelBuilder.Entity<Payment>()
                .HasIndex(p => p.PaymentMethod)
                .HasDatabaseName("IX_Payment_PaymentMethod");

            modelBuilder.Entity<Advance>()
                .HasIndex(a => a.SupplierId)
                .HasDatabaseName("IX_Advance_SupplierId");

            modelBuilder.Entity<Debt>()
                .HasIndex(d => d.SupplierId)
                .HasDatabaseName("IX_Debt_SupplierId");

            modelBuilder.Entity<Incentive>()
                .HasIndex(i => i.SupplierId)
                .HasDatabaseName("IX_Incentive_SupplierId");

            modelBuilder.Entity<Incentive>()
                .HasIndex(i => i.Month)
                .HasDatabaseName("IX_Incentive_Month");

            modelBuilder.Entity<GreenLeafData>()
                .HasIndex(g => g.SupplierId)
                .HasDatabaseName("IX_GreenLeafData_SupplierId");

            modelBuilder.Entity<GreenLeafData>()
                .HasIndex(g => g.CreatedDate)
                .HasDatabaseName("IX_GreenLeafData_CreatedDate");

            // Configure entities from the second context
            modelBuilder.Entity<DenaturedTea>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TeaGrade).IsRequired().HasMaxLength(50);
                entity.Property(e => e.QuantityKg).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);
                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<TeaReturn>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Season).IsRequired().HasMaxLength(100);
                entity.Property(e => e.GardenMark).IsRequired().HasMaxLength(100);
                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.KilosReturned).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ReturnDate).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Season).IsRequired().HasMaxLength(100);
                entity.Property(e => e.GardenMark).IsRequired().HasMaxLength(100);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // Seed data for Invoice
            //modelBuilder.Entity<Invoice>().HasData(
            //    new Invoice { Id = 1, InvoiceNumber = "INV-2024-001", Season = "Spring 2024", GardenMark = "Garden A", InvoiceDate = DateTime.Now.AddDays(-30), TotalAmount = 15000 },
            //    new Invoice { Id = 2, InvoiceNumber = "INV-2024-002", Season = "Summer 2024", GardenMark = "Garden B", InvoiceDate = DateTime.Now.AddDays(-25), TotalAmount = 18000 },
            //    new Invoice { Id = 3, InvoiceNumber = "INV-2024-003", Season = "Autumn 2024", GardenMark = "Garden C", InvoiceDate = DateTime.Now.AddDays(-20), TotalAmount = 22000 }
            //);
        }
    }
}
