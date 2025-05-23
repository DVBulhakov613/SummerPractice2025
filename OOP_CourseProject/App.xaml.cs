using Class_Lib;
using Class_Lib.Backend.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OOP_CourseProject.Controls;
using OOP_CourseProject.Controls.EmployeeControl;
using OOP_CourseProject.Controls.ReceivePackageControls;
using OOP_CourseProject.Controls.SendPackageControls; // REMOVE LATER
using OOP_CourseProject.Controls.UserObjectControls;
using System.Windows;

namespace OOP_CourseProject
{
    public partial class App : Application
    {
        // static instance of the DbContextFactory, used to create DbContext instances
        public static AppDbContextFactory DbContextFactory { get; private set; }

        // used to store the currently logged-in employee
        public static User CurrentEmployee { get; internal set; }

        public static IHost AppHost { get; private set; }
        public static IHost LoginHost { get; private set; } // temp for login


        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => ConfigureAppServices(services, CurrentEmployee))
                .Build();

        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // build a temporary host for login
            LoginHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var connectionString = "Server=localhost\\SQLEXPRESS;Database=PackageDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(connectionString));

                    services.AddBackendServices(null); // no user context yet
                })
                .Build();


            await LoginHost.StartAsync();

            var loginWindow = new LoginWindow();
            bool? loginResult = loginWindow.ShowDialog();

            await LoginHost.StopAsync();
            LoginHost.Dispose();
            LoginHost = null;

            if (loginResult != true || CurrentEmployee == null)
            {
                Shutdown();
                return;
            }

            // main app host with logged-in user context
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => ConfigureAppServices(services, CurrentEmployee))
                .Build();


            await AppHost.StartAsync();

            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            Current.MainWindow = mainWindow;
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            mainWindow.Show();
        }


        private static void ConfigureAppServices(IServiceCollection services, User? employee)
        {
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=PackageDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddBackendServices(employee);

            services.AddSingleton<MainWindow>();
            services.AddTransient<HomePageControl>();
            services.AddTransient<SendPackage>();
            services.AddTransient<ReceivePackageControl>();
            services.AddTransient<EmployeesControl>();
            services.AddTransient<ClientsControl>();
            services.AddTransient<LocationsControl>();
            services.AddTransient<QueryBuilder>();
            services.AddTransient<RolesControl>();
            services.AddTransient<UserObjectControl>();
            //services.AddTransient<PackageConfigurations>();// DEBUG LINE
            //services.AddTransient<ReceivePackageControl>();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            AppHost.Dispose();
            base.OnExit(e);
        }
    }

}
