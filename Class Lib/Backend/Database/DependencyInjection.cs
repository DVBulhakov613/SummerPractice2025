using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Location_related.Methods;
using Class_Lib.Backend.Package_related.Methods;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Class_Lib.Backend.Database
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBackendServices(this IServiceCollection services, string connectionString)
        {
            // register DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // register Repositories
            services.AddScoped<EmployeeRepository>();
            services.AddScoped<ClientRepository>();
            services.AddScoped<LocationRepository>();
            services.AddScoped<PackageRepository>();
            services.AddScoped<ContentRepository>();
            services.AddScoped<PackageEventRepository>();

            // register Services
            services.AddScoped<EmployeeMethods>();
            services.AddScoped<ClientMethods>();
            services.AddScoped<LocationMethods>();
            services.AddScoped<PackageMethods>();
            services.AddScoped<ContentMethods>();
            services.AddScoped<UserMethods>();

            // register QueryBuilderService (generic)
            services.AddScoped(typeof(QueryBuilderService<>));
            return services;
        }
    }
}
