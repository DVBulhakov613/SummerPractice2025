using Class_Lib;
using Class_Lib.Backend.Database;
using Class_Lib.Backend.Location_related.Methods;
using Class_Lib.Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Class_Lib
{
    class Query
    {
        static async Task Main()
        {
            // Console.WriteLine(PasswordHelper.HashPassword("admin"));

            await InsertPostalOffice();
        }

        public static async Task InsertPostalOffice()
        {
            var services = new ServiceCollection();

            string connectionString = "Server=localhost\\SQLEXPRESS;Database=PackageDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

            var employee = new Employee("dummy", "dummy", "+380955548027", "dummy@dummy.com", 1, null);

            services.AddBackendServices(employee);

            var provider = services.BuildServiceProvider();

            var context = provider.GetRequiredService<AppDbContext>();

            PostalOffice po = new PostalOffice(
                new Coordinates(49.807647, 36.053660, "вул. Дніпропетровська", "Харківська область, Мерефа"),
                1000, false, true, false);

            await context.PostalOffices.AddAsync(po);
            await context.SaveChangesAsync();

            Console.WriteLine("Postal office added.");
        }
    }
}
