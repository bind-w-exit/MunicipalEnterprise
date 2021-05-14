using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MunicipalEnterprise.Data.Extensions
{
    public static class DbExtensions
    {
        /// <summary>
        /// This method checks that all migrations have been applied.
        /// </summary>
        public static bool AllMigrationsApplied(this DbContext context)
        {
            try
            {
                var applied = context.GetService<IHistoryRepository>()
                    .GetAppliedMigrations()
                    .Select(m => m.MigrationId);

                var total = context.GetService<IMigrationsAssembly>()
                    .Migrations
                    .Select(m => m.Key);

                return !total.Except(applied).Any();
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }
    }
}
