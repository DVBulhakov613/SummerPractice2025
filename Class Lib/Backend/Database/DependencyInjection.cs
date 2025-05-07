using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Location_related.Methods;
using Class_Lib.Backend.Package_related.Methods;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Class_Lib.Backend.Database
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBackendServices(this IServiceCollection services, string connectionString, Employee currentEmployee)
        {
            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));


            // Repositories
            services.AddScoped<EmployeeRepository>(sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();
                return new EmployeeRepository(db, currentEmployee);
            });

            services.AddScoped<ClientRepository>(sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();
                return new ClientRepository(db, currentEmployee);
            });

            services.AddScoped<LocationRepository>(sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();
                return new LocationRepository(db, currentEmployee);
            });

            services.AddScoped<PackageRepository>(sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();
                return new PackageRepository(db, currentEmployee);
            });

            services.AddScoped<ContentRepository>(sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();
                return new ContentRepository(db, currentEmployee);
            });

            services.AddScoped<PackageEventRepository>(sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();
                return new PackageEventRepository(db, currentEmployee);
            });

            services.AddScoped<UserRepository>(sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();
                return new UserRepository(db);
            });

            services.AddScoped<RoleRepository>(sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();
                return new RoleRepository(db);
            });


            // Services
            services.AddScoped<EmployeeMethods>(sp =>
            {
                var employeeRepo = sp.GetRequiredService<EmployeeRepository>();
                var roleRepo = sp.GetRequiredService<RoleRepository>();
                var roleService = sp.GetRequiredService<RoleService>();
                return new EmployeeMethods(employeeRepo,roleRepo,roleService);
            });

            services.AddScoped<ClientMethods>(sp =>
            {
                var repo = sp.GetRequiredService<ClientRepository>();
                return new ClientMethods(repo);
            });

            services.AddScoped<LocationMethods>(sp =>
            {
                var repo = sp.GetRequiredService<LocationRepository>();
                return new LocationMethods(repo);
            });

            services.AddScoped<PackageMethods>(sp =>
            {
                var repo = sp.GetRequiredService<PackageRepository>();
                return new PackageMethods(repo);
            });

            services.AddScoped<ContentMethods>(sp =>
            {
                var repo = sp.GetRequiredService<ContentRepository>();
                return new ContentMethods(repo);
            });

            services.AddScoped<UserMethods>(sp =>
            {
                var repo = sp.GetRequiredService<UserRepository>();
                return new UserMethods(repo);
            });

            // services
            services.AddScoped(typeof(QueryBuilderService<>)); // generic
            services.AddScoped<RoleService>(sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();
                return new RoleService(db);
            });

            return services;
        }
    }
}
