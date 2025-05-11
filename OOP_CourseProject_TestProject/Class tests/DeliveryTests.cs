using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Package_related.Methods;
using Microsoft.Extensions.DependencyInjection;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Person_related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class_Lib;

namespace OOP_CourseProject_TestProject.Class_tests
{
    [TestClass]
    public class DeliveryTests : TestTemplate
    {
        private DeliveryMethods _deliveryMethods;
        private DeliveryRepository _deliveryRepository;

        private Warehouse _sentTo;
        private Warehouse _sentFrom;
        private Client _sender;
        private Client _receiver;

        [TestInitialize]
        public void Initialize() 
        {
            base.Setup();
            _deliveryMethods = _provider.GetRequiredService<DeliveryMethods>();
            _deliveryRepository = _provider.GetRequiredService<DeliveryRepository>();

            _sentTo = new Warehouse(new Coordinates(55, 5, "Address", "Region"), 50, false);
            _sentFrom = new Warehouse(new Coordinates(50, 50, "Address", "Region"), 50, false);

            _receiver = new Client("First", "Last", "+123456789", "example@example.com");
            _sender = new Client("First", "Last", "+123456789", "example@example.com");
        }

        [TestCleanup]
        public void Clear() => base.Cleanup();

        #region Create
        [TestMethod]
        public async Task CreateDelivery_ShouldCreateDelivery()
        {
            // Arrange
            Package package = new(50, 50, 50, 20, _sender, _receiver, _sentFrom, _sentTo, PackageType.Standard);
            package.AddContent("Cup", ContentType.Miscellaneous, 5);
            package.AddContent("Pan", ContentType.Miscellaneous, 2);
            package.AddContent("Canned food", ContentType.Miscellaneous, 15);
            Delivery delivery = new Delivery(package, _sender, _receiver, _sentFrom, _sentTo, 500, true);

            // Act
            await _deliveryMethods.AddAsync(_adminUser, delivery);
            var createdDeliveries = await _deliveryMethods.GetByCriteriaAsync(_adminUser, d => d.ID == delivery.ID);

            // Assert
            var createdDelivery = createdDeliveries.FirstOrDefault();
            Assert.IsNotNull(createdDelivery);
            Assert.AreEqual(delivery.ID, createdDelivery.ID);
            Assert.AreEqual(delivery.PackageID, createdDelivery.PackageID);
            Assert.AreEqual(delivery.SenderID, createdDelivery.SenderID);
            Assert.AreEqual(delivery.ReceiverID, createdDelivery.ReceiverID);
            Assert.AreEqual(delivery.SentFromID, createdDelivery.SentFromID);
            Assert.AreEqual(delivery.SentToID, createdDelivery.SentToID);
            Assert.AreEqual(delivery.Price, createdDelivery.Price);
            Assert.AreEqual(delivery.IsPaid, createdDelivery.IsPaid);
            Assert.AreEqual(delivery.Timestamp, createdDelivery.Timestamp);
            Assert.AreEqual(delivery.Log.Count, createdDelivery.Log.Count);
        }

        #endregion


        #region Update
        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateCorrectly()
        {
            // Arrange
            Package package = new(50, 50, 50, 20, _sender, _receiver, _sentFrom, _sentTo, PackageType.Standard);
            package.AddContent("Cup", ContentType.Miscellaneous, 5);
            package.AddContent("Pan", ContentType.Miscellaneous, 2);
            package.AddContent("Canned food", ContentType.Miscellaneous, 15);
            Delivery delivery = new Delivery(package, _sender, _receiver, _sentFrom, _sentTo, 500, true);
            await _deliveryMethods.AddAsync(_adminUser, delivery);

            // Act
            delivery.IsPaid = false;
            await _deliveryMethods.UpdateAsync(_adminUser, delivery);
            var updatedDelivery = await _deliveryRepository.GetByIdAsync(delivery.ID);

            // Assert
            Assert.IsNotNull(updatedDelivery);
            Assert.AreEqual(delivery.ID, updatedDelivery.ID);
            Assert.AreEqual(delivery.PackageID, updatedDelivery.PackageID);
            Assert.AreEqual(delivery.SenderID, updatedDelivery.SenderID);
            Assert.AreEqual(delivery.ReceiverID, updatedDelivery.ReceiverID);
            Assert.AreEqual(delivery.SentFromID, updatedDelivery.SentFromID);
            Assert.AreEqual(delivery.SentToID, updatedDelivery.SentToID);
            Assert.AreEqual(delivery.Price, updatedDelivery.Price);
            Assert.AreEqual(delivery.IsPaid, updatedDelivery.IsPaid);
        }
        #endregion

        #region Delete
        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteCorrectly()
        {
            // Arrange
            Package package = new(50, 50, 50, 20, _sender, _receiver, _sentFrom, _sentTo, PackageType.Standard);
            package.AddContent("Cup", ContentType.Miscellaneous, 5);
            package.AddContent("Pan", ContentType.Miscellaneous, 2);
            package.AddContent("Canned food", ContentType.Miscellaneous, 15);
            Delivery delivery = new Delivery(package, _sender, _receiver, _sentFrom, _sentTo, 500, true);
            await _deliveryMethods.AddAsync(_adminUser, delivery);

            // Act
            await _deliveryMethods.DeleteAsync(_adminUser, delivery);
            var deletedDelivery = await _deliveryRepository.GetByIdAsync(delivery.ID);

            // Assert
            Assert.IsNull(deletedDelivery);
        }

        #endregion
    }
}
