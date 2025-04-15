using Class_Lib;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
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
                new object[] { new PostalOffice((Coordinates)CorrectCoordinates.ElementAt(0)[0], 150, true, true, true) },
                new object[] { new PostalOffice((Coordinates)CorrectCoordinates.ElementAt(1)[0], 200, true, true, false) },
                new object[] { new PostalOffice((Coordinates)CorrectCoordinates.ElementAt(2)[0], 250, true, false, true) },
                new object[] { new PostalOffice((Coordinates)CorrectCoordinates.ElementAt(3)[0], 300, true, false, false) },
                new object[] { new PostalOffice((Coordinates)CorrectCoordinates.ElementAt(4)[0], 350, false, true, true) },
                new object[] { new PostalOffice((Coordinates)CorrectCoordinates.ElementAt(5)[0], 400, false, true, false) },
                new object[] { new PostalOffice((Coordinates)CorrectCoordinates.ElementAt(6)[0], 450, false, false, true) },
                new object[] { new PostalOffice((Coordinates)CorrectCoordinates.ElementAt(7)[0], 500, false, false, false) }
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

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _repository = new PackageRepository(_context);
        }

        [TestMethod]
        public async Task AddPackage_ShouldPersistToDatabase()
        {
            // Arrange
            var package = new Package(1, 10, 10, 10, 5, new Client(), new Client(), new Warehouse(), new Warehouse(), new Coordinates(0, 0, "Test", "Region"), new List<Content>(), PackageType.Standard);

            // Act
            await _repository.AddAsync(package);
            var retrievedPackage = await _repository.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(retrievedPackage);
            Assert.AreEqual(package.ID, retrievedPackage.ID);
        }
    }
}