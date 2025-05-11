using Class_Lib;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.Methods;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject_TestProject.Class_tests
{
    [TestClass]
    public class ContentTests : TestTemplate
    {
        private ContentMethods _contentMethods;
        private Package _package;

        [TestInitialize]
        public void Initialize()
        {
            base.Setup();
            _contentMethods = _provider.GetRequiredService<ContentMethods>();
            Warehouse _sentTo = new Warehouse(new Coordinates(55, 5, "Address", "Region"), 50, false);
            Warehouse _sentFrom = new Warehouse(new Coordinates(50, 50, "Address", "Region"), 50, false);

            Client _receiver = new Client("First", "Last", "+123456789", "example@example.com");
            Client _sender = new Client("First", "Last", "+123456789", "example@example.com");

            _package = new Package(10, 10, 10, 5, _sender, _receiver, _sentFrom, _sentTo, PackageType.Standard);
        }

        [TestCleanup]
        public void Clear() => base.Cleanup();


        #region Create
        [TestMethod]
        public async Task AddAsync_ShouldAddContent_WhenUserHasPermission() // also checks GetByCriteriaAsync by proxy
        {
            // Arrange
            Content content = new Content("dummy", ContentType.Miscellaneous, 1, _package);
            // Act
            await _contentMethods.AddAsync(_adminUser, content);
            // Assert
            var contents = await _contentMethods.GetByCriteriaAsync(_adminUser, c => c.Name == "DUMMY");

            Assert.IsTrue(contents.Any());
            Assert.AreEqual(content.Name, contents.First().Name);
            Assert.AreEqual(content.PackageID, contents.First().PackageID);
            Assert.AreEqual(1, contents.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task AddAsync_ShouldThrowException_WhenUserHasNoPermission()
        {
            // Arrange
            Content content = new Content("dummy", ContentType.Miscellaneous, 1, _package);
            Employee dummy = new()
            {
            };
            // Act
            await _contentMethods.AddAsync(_unauth, content);
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateCorrectly()
        {
            // Arrange
            Content content = new Content("dummy", ContentType.Miscellaneous, 1, _package);
            await _contentMethods.AddAsync(_adminUser, content);
            var contents = await _contentMethods.GetByCriteriaAsync(_adminUser, c => c.Name == "DUMMY");

            // Act
            var contentToUpdate = contents.First();
            contentToUpdate.Amount = 2;
            await _contentMethods.UpdateAsync(_adminUser, contentToUpdate);

            // Assert
            var updatedContents = await _contentMethods.GetByCriteriaAsync(_adminUser, c => c.Name == "DUMMY");

            Assert.IsTrue(updatedContents.Any());
            Assert.AreEqual(contentToUpdate.Amount, updatedContents.First().Amount);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task UpdateAsync_ShouldThrowException_WhenNoAuth()
        {
            // Arrange
            Content content = new Content("dummy", ContentType.Miscellaneous, 1, _package);
            await _contentMethods.AddAsync(_adminUser, content);
            var contents = await _contentMethods.GetByCriteriaAsync(_adminUser, c => c.Name == "DUMMY");
            var contentToUpdate = contents.First();
            contentToUpdate.Name = "UpdatedName";

            // Act
            await _contentMethods.UpdateAsync(_unauth, contentToUpdate);

            // Assert
            var updatedContents = await _contentMethods.GetByCriteriaAsync(_unauth, c => c.Name == "UPDATEDNAME");
            Assert.IsFalse(updatedContents.Any());
        }
        #endregion

        #region Delete
        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteCorrectly()
        {
            // Arrange
            Content content = new Content("dummy", ContentType.Miscellaneous, 1, _package);
            await _contentMethods.AddAsync(_adminUser, content);
            var contents = await _contentMethods.GetByCriteriaAsync(_adminUser, c => c.Name == "DUMMY");

            // Act
            var contentToDelete = contents.First();
            await _contentMethods.DeleteAsync(_adminUser, contentToDelete);

            // Assert
            var deletedContents = await _contentMethods.GetByCriteriaAsync(_adminUser, c => c.Name == "DUMMY");
            Assert.IsFalse(deletedContents.Any());
        }

        #endregion
    }
}
