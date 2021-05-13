using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data.Models;

namespace MunicipalEnterprise.Data
{
    public class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=MunicipalEnterprise;Trusted_Connection=True;");
        }

        public MyDbContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
    }
}
