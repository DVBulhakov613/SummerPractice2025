using Class_Lib;
using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Package_related.Methods;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OOP_CourseProject_TestProject.Class_tests;
using System.Diagnostics;
namespace OOP_CourseProject_TestProject
{
    // will do the tests later when i actually figure stuff out

    [TestClass]
    public class PackageRepositoryTests : TestTemplate
    {
        private PackageRepository _repository;
        private PackageMethods _methods;

        private Warehouse _sentTo;
        private Warehouse _sentFrom;
        private Client _sender;
        private Client _receiver;


        [TestInitialize]
        public void Setup()
        {
            base.Setup();
            _repository = _provider.GetRequiredService<PackageRepository>();
            _methods = _provider.GetRequiredService<PackageMethods>();

            _sentTo = new Warehouse(new Coordinates(55, 5, "Address", "Region"), 50, false);
            _sentFrom = new Warehouse(new Coordinates(50, 50, "Address", "Region"), 50, false);

            _receiver = new Client("First", "Last", "+123456789", "example@example.com");
            _sender = new Client("First", "Last", "+123456789", "example@example.com");
        }

        [TestCleanup]
        public void Clear() => base.Cleanup();

        [TestMethod]
        public async Task AddPackage_ShouldPersistToDatabase()
        {
            // arrange
            var warehouseSent = new Warehouse(new Coordinates(0, 0, "Test", "Region"), 500, true);
            var warehouseReceived = new Warehouse(new Coordinates(2, 2, "Test", "Region"), 500, true);
            var employee = new Employee("Name", "Surname", "+380955548027", "example@email.com", "Працівник", warehouseSent);
            var sender = new Client("Name", "Surname", "+380955648027", "example@example.com");
            var receiver = new Client("Name", "Surname", "+380955748027", "example@example.com");

            var package = new Package(10, 10, 10, 5, sender, receiver, warehouseSent, warehouseReceived, new List<Content>(), PackageType.Standard);
            
            // act
            await _methods.AddAsync(_adminUser, package);
            var retrievedPackage = await _repository.GetByIdAsync(1);

            // assert
            Assert.IsNotNull(retrievedPackage);
            Assert.AreEqual(package.ID, retrievedPackage.ID);
        }

        [TestMethod]
        public async Task GetPackagesByStatus_ShouldReturnCorrectPackages()
        {
            // Arrange
            var package1 = new Package(10, 10, 10, 5, null, null, null, null, new List<Content>(), PackageType.Standard)
            {
                PackageStatus = PackageStatus.IN_TRANSIT
            };
            var package2 = new Package(15, 15, 15, 10, null, null, null, null, new List<Content>(), PackageType.Standard)
            {
                PackageStatus = PackageStatus.DELIVERED
            };
            await _repository.AddAsync(package1);
            await _repository.AddAsync(package2);

            // Act
            var inTransitPackages = await _repository.GetByStatusAsync(PackageStatus.IN_TRANSIT);

            // Assert
            Assert.AreEqual(1, inTransitPackages.Count());
            Assert.AreEqual(package1.ID, inTransitPackages.First().ID);
        }

        //[TestMethod]
        //public async Task GetPackagesById_ShouldReturnCorrectPackages()
        //{
        //    // Arrange
        //    var package1 = new Package(10, 10, 10, 5, null, null, null, null, null, new List<Content>(), PackageType.Standard);
        //    var package2 = new Package(15, 15, 15, 10, null, null, null, null, null, new List<Content>(), PackageType.Standard);

        //    await _repository.AddAsync(package1);
        //    await _repository.AddAsync(package2);

        //    // was just checking whether the id is correct
        //    //var packages = await _repository.GetAllAsync();
        //    //Debug.WriteLine(packages.Count());
        //    //await _repository.DeleteAsync(package1);

        //    //packages = await _repository.GetAllAsync();
        //    //Debug.WriteLine(packages.Count());
        //    //await _repository.AddAsync(package1);

        //    // Act
        //    var package1id = package1.ID;
        //    var package = await _repository.GetAllAsync();

        //    // Assert
        //    Assert.AreEqual(package1id, package.FirstOrDefault().ID);
        //}

        #region Create
        //[TestMethod]
        //public async Task AddPackage_ShouldPersistToDatabase()
        //{
        //    // Arrange
        //    var package = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);

        //    // Act
        //    await _repository.AddAsync(package);
        //    var retrievedPackage = _repository.GetAllAsync().Result;

        //    // Assert
        //    Assert.IsNotNull(retrievedPackage);
        //    Assert.AreEqual(package.ID, retrievedPackage.FirstOrDefault().ID);
        //}

        #endregion


        #region Read

        [TestMethod]
        public async Task ReadPackage_ShouldReturnAllPackages()
        {
            // Arrange
            var package1 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            var package2 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            await _repository.AddAsync(package1);
            await _repository.AddAsync(package2);

            // Act
            var packages = _repository.GetAllAsync().Result;

            // Assert
            Assert.IsNotNull(packages);
            Assert.AreEqual(2, packages.Count());
        }

        [TestMethod]
        public async Task ReadPackage_ShouldReturnPackageById()
        {
            // Arrange
            var package = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            await _repository.AddAsync(package);

            // Act
            var retrievedPackage = _repository.GetByIdAsync(package.ID).Result;

            // Assert
            Assert.IsNotNull(retrievedPackage);
            Assert.AreEqual(package.ID, retrievedPackage.ID);
        }

        [TestMethod]
        public async Task ReadPackage_ShouldReturnPackagesByStatus()
        {
            // Arrange
            var package1 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            var package2 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            await _repository.AddAsync(package1);
            await _repository.AddAsync(package2);

            // Act
            var packages = _repository.GetByTypeAsync(PackageType.File).Result;

            // Assert
            Assert.IsNotNull(packages);
            Assert.AreEqual(0, packages.Count());
        }

        //[TestMethod]
        //public async Task ReadPackage_ShouldReturnPackageByStartingPoint()
        //{
        //    // Arrange
        //    var package1 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    var package2 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    await _repository.AddAsync(package1);
        //    await _repository.AddAsync(package2);

        //    // Act
        //    var packages = _repository.GetByStartingPointAsync(_sentFrom).Result;

        //    // Assert
        //    Assert.IsNotNull(packages);
        //    Assert.AreEqual(2, packages.Count());
        //}

        //[TestMethod]
        //public async Task ReadPackage_ShouldReturnPackageByDestination()
        //{
        //    // Arrange
        //    var package1 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    var package2 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    await _repository.AddAsync(package1);
        //    await _repository.AddAsync(package2);

        //    // Act
        //    var packages = _repository.GetByDestinationAsync(_sentTo).Result;

        //    // Assert
        //    Assert.IsNotNull(packages);
        //    Assert.AreEqual(2, packages.Count());
        //}

        //[TestMethod]
        //public async Task ReadPackage_ShouldReturnPackagesBySender()
        //{
        //    // Arrange
        //    var package1 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    var package2 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    await _repository.AddAsync(package1);
        //    await _repository.AddAsync(package2);

        //    // Act
        //    var packages = _repository.GetBySenderAsync(_sender).Result;
        //    // Assert
        //    Assert.IsNotNull(packages);
        //    Assert.AreEqual(2, packages.Count());
        //}

        //[TestMethod]
        //public async Task ReadPackage_ShouldReturnPackagesByReceiver()
        //{
        //    // Arrange
        //    var package1 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    var package2 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    await _repository.AddAsync(package1);
        //    await _repository.AddAsync(package2);

        //    // Act
        //    var packages = _repository.GetByReceiverAsync(_receiver).Result;

        //    // Assert
        //    Assert.IsNotNull(packages);
        //    Assert.AreEqual(2, packages.Count());
        //}

        [TestMethod]
        public async Task ReadPackage_ShouldReturnPackagesByType()
        {
            // Arrange
            var package1 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.File);
            var package2 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            await _repository.AddAsync(package1);
            await _repository.AddAsync(package2);

            // Act
            var packages = _repository.GetByTypeAsync(PackageType.Standard).Result;

            // Assert
            Assert.IsNotNull(packages);
            Assert.AreEqual(1, packages.Count());
        }

        [TestMethod]
        public async Task ReadPackage_ShouldReturnPackagesByWeight()
        {
            // Arrange
            var package1 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            var package2 = new Package(10, 10, 10, 15, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            await _repository.AddAsync(package1);
            await _repository.AddAsync(package2);

            // Act
            var packages = _repository.GetByWeightAsync(5).Result;

            // Assert
            Assert.IsNotNull(packages);
            Assert.AreEqual(1, packages.Count());
        }

        [TestMethod]
        public async Task ReadPackage_ShouldReturnPackagesByWeightRange()
        {
            // Arrange
            var package1 = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            var package2 = new Package(10, 10, 10, 15, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            await _repository.AddAsync(package1);
            await _repository.AddAsync(package2);

            // Act
            var packages = _repository.GetByWeightRangeAsync(5, 15).Result;

            // Assert
            Assert.IsNotNull(packages);
            Assert.AreEqual(2, packages.Count());
        }

        //[TestMethod]
        //public async Task ReadPackage_ShouldReturnPackagesByVolume()
        //{
        //    // Arrange
        //    var package1 = new Package(10, 5, 10, 15, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    var package2 = new Package(10, 10, 10, 15, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    await _repository.AddAsync(package1);
        //    await _repository.AddAsync(package2);

        //    // Act
        //    var packages = _repository.GetByVolumeAsync(1000).Result;

        //    // Assert
        //    Assert.IsNotNull(packages);
        //    Assert.AreEqual(1, packages.Count());
        //}

        //[TestMethod]
        //public async Task ReadPackage_ShouldReturnPackagesByVolumeRange()
        //{
        //    // Arrange
        //    var package1 = new Package(5, 5, 5, 15, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    var package2 = new Package(10, 10, 10, 15, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
        //    await _repository.AddAsync(package1);
        //    await _repository.AddAsync(package2);

        //    // Act
        //    var packages = _repository.GetByVolumeRangeAsync(500, 1500).Result;

        //    // Assert
        //    Assert.IsNotNull(packages);
        //    Assert.AreEqual(1, packages.Count());
        //}

        [TestMethod]
        public async Task ReadPackage_ShouldReturnPackagesByCreationDate()
        {
            // Arrange
            var package1 = new Package(10, 10, 10, 15, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            var package2 = new Package(10, 10, 10, 15, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            await _repository.AddAsync(package1);
            await _repository.AddAsync(package2);

            // Act
            var packages = _repository.GetByCreationDateAsync(DateTime.Now).Result;

            // Assert
            Assert.IsNotNull(packages);
            Assert.AreEqual(2, packages.Count());
        }

        [TestMethod]
        public async Task ReadPackage_ShouldReturnPackagesByCreationDateRange()
        {
            // Arrange
            var package1 = new Package(10, 10, 10, 15, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            var package2 = new Package(10, 10, 10, 15, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            await _repository.AddAsync(package1);
            await _repository.AddAsync(package2);

            // Act
            var packages = _repository.GetByCreationDateRangeAsync(DateTime.Now.AddDays(-1), DateTime.Now).Result;

            // Assert
            Assert.IsNotNull(packages);
            Assert.AreEqual(2, packages.Count());
        }
        #endregion



        #region Update
        [TestMethod]
        public async Task UpdatePackage_ShouldUpdateInDatabase()
        {
            // Arrange
            var package = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            await _repository.AddAsync(package);

            // Act
            package.Weight = 15;
            await _repository.UpdateAsync(package);
            var retrievedPackage = _repository.GetByIdAsync(package.ID).Result;

            // Assert
            Assert.IsNotNull(retrievedPackage);
            Assert.AreEqual(15, retrievedPackage.Weight);
        }

        #endregion

        #region Delete
        [TestMethod]
        public async Task DeletePackage_ShouldRemoveFromDatabase()
        {
            // Arrange
            var package = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);
            await _repository.AddAsync(package);

            // Act
            await _repository.DeleteAsync(package);
            var retrievedPackage = _repository.GetAllAsync().Result;

            // Assert
            Assert.IsFalse(retrievedPackage.Any(p => p.ID == package.ID));
        }

        #endregion
    }

    //[TestClass]
    //public class PersonRepositoryTests
    //{
    //    private AppDbContext _context;
    //    private ClientRepository _clientRepository;
    //    private EmployeeRepository _employeeRepository;


    //    [TestInitialize]
    //    public void Setup()
    //    {
    //        var options = new DbContextOptionsBuilder<AppDbContext>()
    //            .EnableSensitiveDataLogging()
    //            .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
    //            .Options;

    //        _context = new AppDbContext(options);
    //        _clientRepository = new ClientRepository(_context);
    //        _employeeRepository = new EmployeeRepository(_context);
    //        _methods = new PersonMethods(_employeeRepository, _clientRepository);
    //    }

    //    [TestCleanup]
    //    public void Cleanup()
    //    {
    //        _context.Dispose();
    //    }

    //    [TestMethod]
    //    public async Task AddClient_ShouldPersistToDatabase()
    //    {
    //        // Arrange
    //        var client = new Client("John", "Doe", "+123456789");

    //        // Act
    //        await _clientRepository.AddAsync(client);
    //        var retrievedClient = await _clientRepository.GetByIdAsync(1);

    //        // Assert
    //        Assert.IsNotNull(retrievedClient);
    //        Assert.AreEqual(client.ID, retrievedClient.ID);
    //    }

    //    [TestMethod]
    //    public async Task GetClientsByName_ShouldReturnCorrectClients()
    //    {
    //        // Arrange
    //        var client1 = new Client("John", "Doe", "+123456789");
    //        var client2 = new Client("Jane", "Doe", "+987654321");
    //        await _clientRepository.AddAsync(client1);
    //        await _clientRepository.AddAsync(client2);

    //        // Act
    //        var clients = await _clientRepository.GetClientsByLastNameAsync("Doe");

    //        // Assert
    //        Assert.AreEqual(2, clients.Count());
    //    }



    //}
}