using Class_Lib;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using Microsoft.EntityFrameworkCore;
namespace OOP_CourseProject_TestProject
{
    // will do the tests later when i actually figure stuff out
    public static class TestUtilities
    {
        public static IEnumerable<object[]> CorrectPersonNames => new List<object[]>
            {
                new object[] { "Артур" },
                new object[] { "Данило" },
                new object[] { "Віталій" },
                new object[] { "Сергій" },
                new object[] { "Дмитро" }
            };

        public static IEnumerable<object[]> InorrectPersonNames => new List<object[]>
            {
                new object[] { null },
                new object[] { "" },
                new object[] { "   " },
                new object[] { "1" },
                new object[] { "1прфпу" },
                new object[] { ".?%:%(*)" }
            };

        // now invalid; changed to UINT for simplicity and querying as im now using a db

        //public static IEnumerable<object[]> CorrectPackageID => new List<object[]>
        //    {
        //        new object[] { $"00000UA-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}-AAAA" },
        //        new object[] { $"15826UA-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}-XYZX" },
        //        new object[] { $"61000UA-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}-ABCD" },
        //        new object[] { $"61000UA-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}-AAAA" }
        //    };

        //public static IEnumerable<object[]> IncorrectPackageID => new List<object[]>
        //    {
        //        new object[] { null },
        //        new object[] { "" },
        //        new object[] { " " },
        //        new object[] { "1" },
        //        new object[] { "something something" },
        //        new object[] { "-15000UA-00-00-0000-XYZX" },
        //        new object[] { "61O00UA-00-00-0000-ABCD" },
        //        new object[] { "61000UA-00-0000-AAAA" },
        //        new object[] { "61000UA-00-0000-0000" }
        //    };

        public static IEnumerable<object[]> CorrectCoordinates => new List<object[]>
            {
                // all are valid
                new object[] { new Coordinates(50.4501, 30.5234, "Khreshchatyk St, Kyiv, 01001", "Kyiv" ) },
                new object[] { new Coordinates(49.8397, 24.0297, "Svobody Ave, Lviv, 79000", "Lviv" ) },
                new object[] { new Coordinates(48.4647, 35.0462, "Dnipro City Center, Dnipro, 49000", "Dnipropetrovsk" ) },
                new object[] { new Coordinates(46.4825, 30.7233, "Deribasivska St, Odesa, 65000", "Odesa" ) },
                new object[] { new Coordinates(47.8388, 35.1396, "Soborna St, Zaporizhzhia, 69000", "Zaporizhzhia" ) },
                new object[] { new Coordinates(50.9077, 34.7981, "Peremohy Square, Sumy, 40000", "Sumy" ) },
                new object[] { new Coordinates(49.9935, 36.2304, "Sumska St, Kharkiv, 61000", "Kharkiv" ) },
                new object[] { new Coordinates(48.6208, 22.2879, "Narodna Sq, Uzhhorod, 88000", "Zakarpattia" ) },
                new object[] { new Coordinates(49.5535, 25.5948, "Shevchenka Blvd, Ternopil, 46000", "Ternopil" ) },
                new object[] { new Coordinates(48.9215, 24.7097, "Nezalezhnosti St, Ivano-Frankivsk, 76000", "Ivano - Frankivsk" ) }
            };

        public static IEnumerable<object[]> IncorrectCoordinates => new List<object[]>
            {
                // latitude out of range (> 90)
                new object[] { new Coordinates(95.0000, 30.5234, "Khreshchatyk St, Kyiv, 01001", "Kyiv" ) },

                // longitude out of range (< -180)
                new object[] { new Coordinates(50.4501, -190.0000, "Khreshchatyk St, Kyiv, 01001", "Kyiv" ) },

                // mismatched city and coordinates (Kyiv address with Lviv coordinates)
                new object[] { new Coordinates(49.8397, 24.0297, "Khreshchatyk St, Kyiv, 01001", "Kyiv" ) },

                // coordinates in the ocean, address says Odesa
                new object[] { new Coordinates(0.0000, -140.0000, "Deribasivska St, Odesa, 65000", "Odesa" ) },

                // typo in city name (kharkov instead of kharkiv), and bad lat/lon
                new object[] { new Coordinates(120.0000, 200.0000, "Sumska St, Kharkov, 61000", "Kharkov" ) },

                // valid coordinates but for a different country
                new object[] { new Coordinates(51.5074, -0.1278, "Sumska St, Kharkiv, 61000", "Kharkiv" ) }, // Coordinates for London

                // swapped lat/lon values
                new object[] { new Coordinates(30.5234, 50.4501, "Khreshchatyk St, Kyiv, 01001", "Kyiv" ) },

                // completely invalid lat/lon (non-existent values)
                new object[] { new Coordinates(-999.999, 999.999, "Shevchenka Blvd, Ternopil, 46000", "Ternopil" ) },

                // empty address but valid coordinates
                new object[] { new Coordinates(49.9935, 36.2304, "", "Kharkiv" ) },

                // mismatched region and city
                new object[] { new Coordinates(48.4647, 35.0462, "Dnipro City Center, Dnipro, 49000", "Lviv" ) }
            };

        public static IEnumerable<object[]> CorrectPostalOffice => new List<object[]>
            {
                new object[] { new PostalOffice(10, (Coordinates)CorrectCoordinates.ElementAt(0)[0], 150, true, true, true) },
                new object[] { new PostalOffice(11, (Coordinates)CorrectCoordinates.ElementAt(1)[0], 200, true, true, false) },
                new object[] { new PostalOffice(12, (Coordinates)CorrectCoordinates.ElementAt(2)[0], 250, true, false, true) },
                new object[] { new PostalOffice(13, (Coordinates)CorrectCoordinates.ElementAt(3)[0], 300, true, false, false) },
                new object[] { new PostalOffice(14, (Coordinates)CorrectCoordinates.ElementAt(4)[0], 350, false, true, true) },
                new object[] { new PostalOffice(15, (Coordinates)CorrectCoordinates.ElementAt(5)[0], 400, false, true, false) },
                new object[] { new PostalOffice(16, (Coordinates)CorrectCoordinates.ElementAt(6)[0], 450, false, false, true) },
                new object[] { new PostalOffice(17, (Coordinates)CorrectCoordinates.ElementAt(7)[0], 500, false, false, false) }
            };
    }

    [TestClass]
    public sealed class ObjectCreation
    {
        [TestMethod]
        public void CreatePackage()
        {
        }
    }

    [TestClass]
    public class AccessServiceTests
    {
        // to-do: rewrite this to use TestUtility and abuse the dynamicdata property
        [TestMethod]
        public void CanPerformAction_ValidRoleAndAction_ReturnsTrue()
        {
            // arrange
            var role = typeof(Administrator);
            var action = "CreatePackage";

            // act
            var result = AccessService.CanPerformAction(role, action);

            // assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CanPerformAction_InvalidRoleAndAction_ReturnsFalse()
        {
            // arrange
            var role = typeof(Employee);
            var action = "DeletePackage";

            // act
            var result = AccessService.CanPerformAction(role, action);

            // assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanPerformAction_NonExistentAction_ReturnsFalse()
        {
            // arrange
            var role = typeof(Administrator);
            var action = "NonExistentAction";

            // act
            var result = AccessService.CanPerformAction(role, action);

            // assert
            Assert.IsFalse(result);
        }
    }

    [TestClass]
    public class PackageRepositoryTests
    {
        private AppDbContext _context;
        private PackageRepository _repository;
        private PackageMethods _methods;


        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _repository = new PackageRepository(_context);
            _methods = new PackageMethods(_repository);
        }

        [TestMethod]
        public async Task AddPackage_ShouldPersistToDatabase()
        {
            // arrange
            var warehouseSent = new Warehouse(0, new Coordinates(0, 0, "Test", "Region"), 500, true);
            var warehouseReceived = new Warehouse(0, new Coordinates(2, 2, "Test", "Region"), 500, true);
            var employee = new Employee(0, "Name", "Surname", "+380955548027", "Clerk", warehouseSent);
            var sender = new Client(0, "Name", "Surname", "+380955648027");
            var receiver = new Client(1, "Name", "Surname", "+380955748027");

            var package = new Package(0, 10, 10, 10, 5, sender, receiver, warehouseSent, warehouseReceived, new Coordinates(3, 3, "Test", "Region"), new List<Content>(), PackageType.Standard);
            
            // act
            await _methods.AddPackageAsync(employee, package, sender, receiver);
            var retrievedPackage = await _repository.GetByIdAsync(1);

            // assert
            Assert.IsNotNull(retrievedPackage);
            Assert.AreEqual(package.ID, retrievedPackage.ID);
        }

        [TestMethod]
        public async Task GetPackagesByStatus_ShouldReturnCorrectPackages()
        {
            // Arrange
            var package1 = new Package(1, 10, 10, 10, 5, null, null, null, null, null, new List<Content>(), PackageType.Standard)
            {
                PackageStatus = PackageStatus.IN_TRANSIT
            };
            var package2 = new Package(2, 15, 15, 15, 10, null, null, null, null, null, new List<Content>(), PackageType.Standard)
            {
                PackageStatus = PackageStatus.DELIVERED
            };
            await _repository.AddAsync(package1);
            await _repository.AddAsync(package2);

            // Act
            var inTransitPackages = await _repository.GetPackagesByStatusAsync(PackageStatus.IN_TRANSIT);

            // Assert
            Assert.AreEqual(1, inTransitPackages.Count());
            Assert.AreEqual(package1.ID, inTransitPackages.First().ID);
        }

    }

    [TestClass]
    public class PersonRepositoryTests
    {
        private AppDbContext _context;
        private ClientRepository _clientRepository;
        private EmployeeRepository _employeeRepository;
        private PersonMethods _methods;


        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _clientRepository = new ClientRepository(_context);
            _employeeRepository = new EmployeeRepository(_context);
            _methods = new PersonMethods(_employeeRepository, _clientRepository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [TestMethod]
        public async Task AddClient_ShouldPersistToDatabase()
        {
            // Arrange
            var client = new Client(0, "John", "Doe", "+123456789");

            // Act
            await _clientRepository.AddAsync(client);
            var retrievedClient = await _clientRepository.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(retrievedClient);
            Assert.AreEqual(client.ID, retrievedClient.ID);
        }

        [TestMethod]
        public async Task GetClientsByName_ShouldReturnCorrectClients()
        {
            // Arrange
            var client1 = new Client(1, "John", "Doe", "+123456789");
            var client2 = new Client(2, "Jane", "Doe", "+987654321");
            await _clientRepository.AddAsync(client1);
            await _clientRepository.AddAsync(client2);

            // Act
            var clients = await _clientRepository.GetClientsByLastNameAsync("Doe");

            // Assert
            Assert.AreEqual(2, clients.Count());
        }

    }
}