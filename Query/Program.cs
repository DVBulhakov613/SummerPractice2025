using Class_Lib;
using Class_Lib.Backend.Database;
using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Location_related.Methods;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.Methods;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Backend.Serialization;
using Class_Lib.Backend.Serialization.DTO;
using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Class_Lib
{
    class Query
    {
        static async Task Main()
        {
            //Console.WriteLine(PasswordHelper.HashPassword("anatoliy.mikhailov"));
            await SerializationTesting("C:\\Users\\User\\source\\repos\\OOP_CourseProject\\employee.json");

            // await InsertPostalOffice();
        }

        //public static async Task InsertPostalOffice()
        //{
        //    var services = new ServiceCollection();

        //    string connectionString = "Server=localhost\\SQLEXPRESS;Database=PackageDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

        //    var employee = new Employee("dummy", "dummy", "+380955548027", "dummy@dummy.com", null);

        //    services.AddBackendServices(employee);

        //    var provider = services.BuildServiceProvider();

        //    var context = provider.GetRequiredService<AppDbContext>();

        //    PostalOffice po = new PostalOffice(
        //        new Coordinates(49.807647, 36.053660, "вул. Дніпропетровська", "Харківська область, Мерефа"),
        //        1000, false, true, false);

        //    await context.PostalOffices.AddAsync(po);
        //    await context.SaveChangesAsync();

        //    Console.WriteLine("Postal office added.");
        //}

        public static async Task SerializationTesting(string filepath)
        {
            Employee currentEmployee = new("test", "test", "+380000000000", "a@a.com", null);
            User user = new("test", "test", new Role { ID = 1, Name = "Системний Адміністратор" }, currentEmployee);
            user.RoleID = 1;

            var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                var connectionString = "Server=localhost\\SQLEXPRESS;Database=PackageDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString));

                services.AddBackendServices(user); // skip user context
            })
            .Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            // login

            var userRepository = services.GetRequiredService<UserRepository>();
            var roleService = services.GetRequiredService<RoleService>();
            var userMethods = services.GetRequiredService<UserMethods>();


            await roleService.CachePermissionsAsync(user);

            Console.WriteLine($"Logged in as: {user.Username}");



            // user serialization
            var serializeUser = await userRepository.GetByUsernameAsync("anatoliy.mikhailov");
            await roleService.CachePermissionsAsync(serializeUser);

            var userDto = DtoMapper.ToDto(serializeUser);
            JsonSerialization.SaveToFile(userDto, "C:\\Users\\User\\source\\repos\\OOP_CourseProject\\user.json");

            UserDTO? loadUser = JsonSerialization.LoadFromFile<UserDTO>("C:\\Users\\User\\source\\repos\\OOP_CourseProject\\user.json");

            User deserializedUser = DtoMapper.ToEntity(loadUser);

            Console.WriteLine(deserializedUser.Username);


            // employee serialization
            var serializeEmployee = await services.GetRequiredService<EmployeeMethods>().GetByCriteriaAsync(user, e => e.ID == 10);

            var employeeDto = DtoMapper.ToDto(serializeEmployee.First());
            JsonSerialization.SaveToFile(employeeDto, "C:\\Users\\User\\source\\repos\\OOP_CourseProject\\employee.json");

            EmployeeDTO? loadEmployee = JsonSerialization.LoadFromFile<EmployeeDTO>("C:\\Users\\User\\source\\repos\\OOP_CourseProject\\employee.json");

            Employee deserializedEmployee = DtoMapper.ToEntity(loadEmployee);

            Console.WriteLine(deserializedUser.Username);



            // Delivery serialization
            var serializeDelivery = await services.GetRequiredService<DeliveryMethods>().GetByCriteriaAsync(user, e => e.ID == 52);

            var DeliveryDto = DtoMapper.ToDto(serializeDelivery.First());
            JsonSerialization.SaveToFile(DeliveryDto, "C:\\Users\\User\\source\\repos\\OOP_CourseProject\\delivery.json");

            DeliveryDTO? loadDelivery = JsonSerialization.LoadFromFile<DeliveryDTO>("C:\\Users\\User\\source\\repos\\OOP_CourseProject\\delivery.json");

            Delivery deserializedDelivery = DtoMapper.ToEntity(loadDelivery);

            Console.WriteLine(deserializedUser.Username);



            // Package serialization
            var serializePackage = await services.GetRequiredService<PackageMethods>().GetByCriteriaAsync(user, e => e.ID == 10);

            var PackageDto = DtoMapper.ToDto(serializePackage.First());
            JsonSerialization.SaveToFile(PackageDto, "C:\\Users\\User\\source\\repos\\OOP_CourseProject\\package.json");

            PackageDTO? loadPackage = JsonSerialization.LoadFromFile<PackageDTO>("C:\\Users\\User\\source\\repos\\OOP_CourseProject\\package.json");

            Package deserializedPackage = DtoMapper.ToEntity(loadPackage);

            Console.WriteLine(deserializedUser.Username);



            // Role serialization
            var serializeRole = await services.GetRequiredService<RoleRepository>().GetByCriteriaAsync(e => e.ID == 1);

            var RoleDto = DtoMapper.ToDto(serializeRole.First());
            JsonSerialization.SaveToFile(RoleDto, "C:\\Users\\User\\source\\repos\\OOP_CourseProject\\role.json");

            RoleDTO? loadRole = JsonSerialization.LoadFromFile<RoleDTO>("C:\\Users\\User\\source\\repos\\OOP_CourseProject\\role.json");

            Role deserializedRole = DtoMapper.ToEntity(loadRole);

            Console.WriteLine(deserializedUser.Username);



            // Client serialization
            var serializeClient = await services.GetRequiredService<ClientMethods>().GetByCriteriaAsync(user, e => e.ID == 2);

            var ClientDto = DtoMapper.ToDto(serializeClient.First());
            JsonSerialization.SaveToFile(ClientDto, "C:\\Users\\User\\source\\repos\\OOP_CourseProject\\client.json");

            ClientDTO? loadClient = JsonSerialization.LoadFromFile<ClientDTO>("C:\\Users\\User\\source\\repos\\OOP_CourseProject\\client.json");

            Client deserializedClient = DtoMapper.ToEntity(loadClient);

            Console.WriteLine(deserializedUser.Username);



            // Content serialization
            var p = await services.GetRequiredService<PackageMethods>().GetByCriteriaAsync(user, p => p.ID == 50);
            Package package = p.First();
            var serializeContent = new Content("test", "test", ContentType.Tools, 1, package);

            var ContentDto = DtoMapper.ToDto(serializeContent);
            JsonSerialization.SaveToFile(ContentDto, "C:\\Users\\User\\source\\repos\\OOP_CourseProject\\content.json");

            ContentDTO? loadContent = JsonSerialization.LoadFromFile<ContentDTO>("C:\\Users\\User\\source\\repos\\OOP_CourseProject\\content.json");

            Content deserializedContent = DtoMapper.ToEntity(loadContent);

            Console.WriteLine(deserializedUser.Username);



            // Location serialization
            var serializeLocation = await services.GetRequiredService<LocationMethods>().GetByCriteriaAsync(user, e => e.ID == 2);

            var LocationDto = DtoMapper.ToDto(serializeLocation.First());
            JsonSerialization.SaveToFile(LocationDto, "C:\\Users\\User\\source\\repos\\OOP_CourseProject\\location.json");

            LocationDTO? loadLocation = JsonSerialization.LoadFromFile<LocationDTO>("C:\\Users\\User\\source\\repos\\OOP_CourseProject\\location.json");

            BaseLocation deserializedLocation = DtoMapper.ToEntity(loadLocation);

            Console.WriteLine(deserializedUser.Username);


            //Console.WriteLine("Serialized Employee:");
            //Console.WriteLine(json);
        }
    }
}
