using Class_Lib.Database.Repositories;
using Class_Lib;
using Microsoft.EntityFrameworkCore;
using Class_Lib.Backend.Person_related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Package_related;
using Microsoft.Extensions.DependencyInjection;

namespace OOP_CourseProject_TestProject.Class_tests
{
    [TestClass]
    public class PackageRepositoryTests : TestTemplate
    {
        private PackageRepository _repository;
        private Warehouse _sentTo;
        private Warehouse _sentFrom;
        private Client _sender;
        private Client _receiver;


        [TestInitialize]
        public void Initialize()
        {
            base.Setup();

            _repository = _provider.GetRequiredService<PackageRepository>();

            _sentTo = new Warehouse(new Coordinates(55, 5, "Address", "Region"), 50, false);
            _sentFrom = new Warehouse(new Coordinates(50, 50, "Address", "Region"), 50, false);

            _receiver = new Client("First", "Last", "+123456789", "example@example.com");
            _sender = new Client("First", "Last", "+123456789", "example@example.com");
        }

        [TestCleanup]
        public void Clear() => base.Cleanup();

        // need to test:
        // CRUD
        #region Create
        [TestMethod]
        public async Task AddPackage_ShouldPersistToDatabase()
        {
            // Arrange
            var package = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, new List<Content>(), PackageType.Standard);

            // Act
            await _repository.AddAsync(package);
            var retrievedPackage = _repository.GetAllAsync().Result;

            // Assert
            Assert.IsNotNull(retrievedPackage);
            Assert.AreEqual(package.ID, retrievedPackage.FirstOrDefault().ID);
        }

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
        // searching by criteria (query builder service, although that can be made elsewhere)
    }
}
