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
                return new EmployeeRepository(sp.GetRequiredService<AppDbContext>(), currentEmployee);
            });

            services.AddScoped<ClientRepository>(sp =>
            {
                return new ClientRepository(sp.GetRequiredService<AppDbContext>(), currentEmployee);
            });

            services.AddScoped<LocationRepository>(sp =>
            {
                return new LocationRepository(sp.GetRequiredService<AppDbContext>(), currentEmployee);
            });

            services.AddScoped<DeliveryRepository>(sp =>
            {
                return new DeliveryRepository(sp.GetRequiredService<AppDbContext>(), currentEmployee);
            });

            services.AddScoped<PackageRepository>(sp =>
            {
                return new PackageRepository(sp.GetRequiredService<AppDbContext>(), currentEmployee);
            });

            services.AddScoped<ContentRepository>(sp =>
            {
                return new ContentRepository(sp.GetRequiredService<AppDbContext>(), currentEmployee);
            });

            services.AddScoped<PackageEventRepository>(sp =>
            {
                return new PackageEventRepository(sp.GetRequiredService<AppDbContext>(), currentEmployee);
            });

            services.AddScoped<UserRepository>(sp =>
            {
                return new UserRepository(sp.GetRequiredService<AppDbContext>());
            });

            services.AddScoped<RoleRepository>(sp =>
            {
                return new RoleRepository(sp.GetRequiredService<AppDbContext>());
            });




            // Services
            services.AddScoped<EmployeeMethods>(sp =>
            {
                return new EmployeeMethods(
                    sp.GetRequiredService<EmployeeRepository>(),
                    sp.GetRequiredService<RoleRepository>(),
                    sp.GetRequiredService<RoleService>()
                    );
            });

            services.AddScoped<ClientMethods>(sp =>
            {
                return new ClientMethods(sp.GetRequiredService<ClientRepository>());
            });

            services.AddScoped<LocationMethods>(sp =>
            {
                return new LocationMethods(sp.GetRequiredService<LocationRepository>(), sp.GetRequiredService<EmployeeMethods>());
            });

            services.AddScoped<DeliveryMethods>(sp =>
            {
                return new DeliveryMethods(sp.GetRequiredService<DeliveryRepository>());
            });

            services.AddScoped<PackageMethods>(sp =>
            {
                return new PackageMethods(sp.GetRequiredService<PackageRepository>());
            });

            services.AddScoped<ContentMethods>(sp =>
            {
                return new ContentMethods(sp.GetRequiredService<ContentRepository>());
            });

            services.AddScoped<UserMethods>(sp =>
            {
                return new UserMethods(sp.GetRequiredService<UserRepository>());
            });

            // services
            services.AddScoped(typeof(QueryBuilderService<>)); // generic
            services.AddScoped<RoleService>(sp =>
            {
                return new RoleService(sp.GetRequiredService<AppDbContext>());
            });

            return services;
        }
    }
}
