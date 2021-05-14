using Microsoft.EntityFrameworkCore;

namespace MunicipalEnterprise.Data
{
    /// <summary>
    /// A factory for creating derived DbContext instances.
    /// </summary>
    public class MyDbContextFactory : IDbContextFactory<MyDbContext>
    {
        /// <summary>
        /// Creates a new instance of a derived DbContext type.
        /// </summary>
        public MyDbContext CreateDbContext()
        {
            return new MyDbContext();
        }
    }
}
