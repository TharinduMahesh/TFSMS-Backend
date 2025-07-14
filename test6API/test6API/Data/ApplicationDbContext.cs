using Microsoft.EntityFrameworkCore;
using test6API.Models;

namespace test6API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }
        public DbSet<GrowerSignUp> GrowerSignUps { get; set; } = null!;
        public DbSet<GrowerCreateAccount> GrowerCreateAccounts { get; set; }
        public DbSet<GrowerOrder> GrowerOrders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<GrowerBankDetail> GrowerBankDetails { get; set; }
        public DbSet<CollectorSignUp> CollectorSignUps { get; set; }
        public DbSet<CollectorCreateAccount> CollectorCreateAccounts { get; set; }
        public DbSet<CollectorBankDetail> CollectorBankDetails { get; set; }
        public DbSet<Fertilizer> Fertilizers { get; set; }
    }
}
