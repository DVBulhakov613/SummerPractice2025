using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Class_Lib
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // Configure the database provider (e.g., SQLite)
            var projectDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..");
            var dbPath = System.IO.Path.Combine(projectDirectory, "app.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
