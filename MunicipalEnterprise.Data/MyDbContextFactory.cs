using Microsoft.EntityFrameworkCore;

namespace MunicipalEnterprise.Data
{
    public class MyDbContextFactory : IDbContextFactory<MyDbContext>
    {
        public MyDbContext CreateDbContext()
        {
            return new MyDbContext();
        }
    }
}
