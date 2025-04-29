using Class_Lib;
using Class_Lib.Backend.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OOP_CourseProject.Controls;
using System.Windows;

namespace OOP_CourseProject
{
    public partial class App : Application
    {
        // static instance of the DbContextFactory, used to create DbContext instances
        public static AppDbContextFactory DbContextFactory { get; private set; }

        // used to store the currently logged-in employee
        public static Employee CurrentEmployee { get; internal set; }

        public static IHost AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Add backend services
                    services.AddBackendServices("Server=localhost\\SQLEXPRESS;Database=PackageDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

                    // Register UI components
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<EmployeeControl>();
                    services.AddTransient<QueryBuilderViewModel>();
                })
                .Build();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            AppHost.Dispose();
            base.OnExit(e);
        }

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);
        //    // currently on localhost because none of this is real :yum:
        //    var connectionString = "Server=localhost\\SQLEXPRESS;Database=PackageDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
        //    DbContextFactory = new AppDbContextFactory(connectionString);
        //}
    }

}
