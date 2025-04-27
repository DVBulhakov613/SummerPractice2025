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
using System.Diagnostics;
namespace OOP_CourseProject_TestProject
{
    // will do the tests later when i actually figure stuff out

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
            var warehouseSent = new Warehouse(new Coordinates(0, 0, "Test", "Region"), 500, true);
            var warehouseReceived = new Warehouse(new Coordinates(2, 2, "Test", "Region"), 500, true);
            var employee = new Employee("Name", "Surname", "+380955548027", "Clerk", warehouseSent);
            var sender = new Client("Name", "Surname", "+380955648027");
            var receiver = new Client("Name", "Surname", "+380955748027");

            var package = new Package(10, 10, 10, 5, sender, receiver, warehouseSent, warehouseReceived, warehouseSent, new List<Content>(), PackageType.Standard);
            
            // act
            await _methods.AddPackageAsync(employee, package);
            var retrievedPackage = await _repository.GetByIdAsync(1);

            // assert
            Assert.IsNotNull(retrievedPackage);
            Assert.AreEqual(package.ID, retrievedPackage.ID);
        }

        [TestMethod]
        public async Task GetPackagesByStatus_ShouldReturnCorrectPackages()
        {
            // Arrange
            var package1 = new Package(10, 10, 10, 5, null, null, null, null, null, new List<Content>(), PackageType.Standard)
            {
                PackageStatus = PackageStatus.IN_TRANSIT
            };
            var package2 = new Package(15, 15, 15, 10, null, null, null, null, null, new List<Content>(), PackageType.Standard)
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