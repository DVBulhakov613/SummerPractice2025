using Class_Lib;
using Class_Lib.Backend.Database;
using System.Configuration;
using System.Data;
using System.Windows;

namespace OOP_CourseProject
{
    public partial class App : Application
    {
        // static instance of the DbContextFactory, used to create DbContext instances
        public static AppDbContextFactory DbContextFactory { get; private set; }

        // used to store the currently logged-in employee
        public static Employee CurrentEmployee { get; internal set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // currently on localhost because none of this is real :yum:
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=PackageDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
            DbContextFactory = new AppDbContextFactory(connectionString);
        }
    }

}
