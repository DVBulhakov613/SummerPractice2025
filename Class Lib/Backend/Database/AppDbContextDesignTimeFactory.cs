using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Class_Lib.Backend.Database
{
    /// <summary>
    /// This factory is used by Entity Framework Core to create instances of AppDbContext at design time.
    /// It is not intended for use in application code.
    /// </summary>
    public class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=PackageDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
