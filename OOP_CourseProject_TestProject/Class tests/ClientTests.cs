using Class_Lib;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject_TestProject.Class_tests
{
    [TestClass]
    public class ClientTests : TestTemplate
    {
        private ClientMethods _clientMethods;

        [TestInitialize]
        public void Initialize()
        {
            base.Setup();
            _clientMethods = _provider.GetRequiredService<ClientMethods>();
        }

        [TestCleanup]
        public void Clear() => base.Cleanup();

        #region Create
        [TestMethod]
        public async Task AddAsync_ShouldAddClient_WhenUserHasPermission() // also checks GetByCriteriaAsync by proxy
        {
            // Arrange
            Client client = new Client("John", "Doe", "+000000000000", "example@example.com");

            // Act
            await _clientMethods.AddAsync(_adminUser, client);

            // Assert
            var clients = await _clientMethods.GetByCriteriaAsync(_adminUser, c => c.FirstName == "John" && c.Surname == "Doe");
            Assert.IsTrue(clients.Any());
            Assert.AreEqual(client.FirstName, clients.First().FirstName);
            Assert.AreEqual(client.Surname, clients.First().Surname);
            Assert.AreEqual(client.PhoneNumber, clients.First().PhoneNumber);
            Assert.AreEqual(client.Email, clients.First().Email);
            Assert.AreEqual(client.ID, clients.First().ID);
            Assert.AreEqual(1, clients.Count());
        }


        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task AddAsync_ShouldThrowException_WhenUserHasNoPermission()
        {
            // Arrange
            Client client = new("John", "Doe", "+000000000000", "example@example.com");
            Employee dummy = new()
            {
                Role = new Role { ID = 999999, Name = "TestRole" }
            };

            // Act + Assert
            await _clientMethods.AddAsync(dummy, client);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task AddAsync_ShouldThrowException_WhenClientIsNull()
        {
            // Arrange
            Client client = null;

            //// Act
            //await _clientMethods.AddAsync(_adminUser, client);

            // Act + Assert
            await _clientMethods.AddAsync(_adminUser, client);
        }

        #endregion

        #region Read

        //[TestMethod]
        //public async Task ReadClientAsync_GetByCriteriaAsync()
        //{
        //    // Arrange
        //    Client client = new("John", "Doe", "+000000000000", "example@example.com");
        //    await _clientMethods.AddAsync(_adminUser, client);

        //    // Act
        //    var clients = await _clientMethods.GetByCriteriaAsync(_adminUser, c => c.FirstName == "John" && c.Surname == "Doe");

        //    // Assert
        //    Assert.IsTrue(clients.Any());
        //    Assert.AreEqual(client.FirstName, clients.First().FirstName);
        //    Assert.AreEqual(client.Surname, clients.First().Surname);
        //    Assert.AreEqual(client.PhoneNumber, clients.First().PhoneNumber);
        //    Assert.AreEqual(client.Email, clients.First().Email);
        //    Assert.AreEqual(client.ID, clients.First().ID);
        //    Assert.AreEqual(1, clients.Count());

        //}

        #endregion



        #region Update
        [TestMethod]
        public async Task UpdateAsync()
        {
            // Arrange
            Client client = new("John", "Doe", "+000000000000", "example@example.com");
            await _clientMethods.AddAsync(_adminUser, client);

            // Act
            client.FirstName = "Jane";
            client.Surname = "Smith";
            client.PhoneNumber = "+111111111111";
            client.Email = "differentexample@example.com";

            await _clientMethods.UpdateAsync(_adminUser, client);

            // Assert
            var updatedClient = await _clientMethods.GetByCriteriaAsync(_adminUser, c => c.ID == client.ID);
            Assert.IsTrue(updatedClient.Any());
            Assert.AreEqual(client.FirstName, updatedClient.First().FirstName);
            Assert.AreEqual(client.Surname, updatedClient.First().Surname);
            Assert.AreEqual(client.PhoneNumber, updatedClient.First().PhoneNumber);
            Assert.AreEqual(client.Email, updatedClient.First().Email);
            Assert.AreEqual(client.ID, updatedClient.First().ID);
            Assert.AreEqual(1, updatedClient.Count());
        }

        #endregion

        #region Delete
        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteClient_WhenUserHasPermission()
        {
            // Arrange
            Client client = new("John", "Doe", "+000000000000", "example@example.com");
            await _clientMethods.AddAsync(_adminUser, client);

            // Act
            await _clientMethods.DeleteAsync(_adminUser, client);

            // Assert
            var deletedClient = await _clientMethods.GetByCriteriaAsync(_adminUser, c => c.ID == client.ID);
            Assert.IsFalse(deletedClient.Any());
            Assert.AreEqual(0, deletedClient.Count());
        }


        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task DeleteAsync_ShouldNotDeleteClient_WhenUserHasNoPermission()
        {
            // Arrange
            Client client = new("John", "Doe", "+000000000000", "example@example.com");
            Employee dummy = new()
            {
                Role = new Role { ID = 999999, Name = "TestRole" }
            };

            await _clientMethods.AddAsync(_adminUser, client);

            // Act
            await _clientMethods.DeleteAsync(dummy, client);

            // Assert
            var deletedClient = await _clientMethods.GetByCriteriaAsync(_adminUser, c => c.ID == client.ID);
            Assert.IsTrue(deletedClient.Any());
            Assert.AreEqual(1, deletedClient.Count());
        }

        #endregion
    }
}
