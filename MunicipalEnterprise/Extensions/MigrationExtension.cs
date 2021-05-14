using Microsoft.EntityFrameworkCore;
using MunicipalEnterprise.Data;
using MunicipalEnterprise.Data.Extensions;

namespace MunicipalEnterprise.Extensions
{
    public static class MigrationExtension
    {
        /// <summary>
        /// This method applies migration programmatically
        /// </summary>
        public static void ApplyMigrations()
        {
            using (var context = new MyDbContext())
            {
                if (!context.AllMigrationsApplied())
                {
                    context.Database.Migrate();
                }
            }            
        }
    }
}
