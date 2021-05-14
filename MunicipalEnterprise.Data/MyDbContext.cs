using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data.Models;

namespace MunicipalEnterprise.Data
{
    public class MyDbContext : DbContext
    {
        /// <summary>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=MunicipalEnterprise;Trusted_Connection=True;");     //These Use* methods are extension methods implemented by the database provider. 
        }

        public DbSet<User> Users { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
    }
}
