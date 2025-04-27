using Class_Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject_TestProject
{
    [TestClass]
    public class CoordinatesTests
    {
        [TestMethod]
        [DynamicData(nameof(TestUtilities.CorrectCoordinates), typeof(TestUtilities), DynamicDataSourceType.Property)]
        public void Coordinates_ValidCoordinates_ShouldCreateInstance(Coordinates coordinates)
        {
            // arrange & act
            var instance = new Coordinates(coordinates.Latitude, coordinates.Longitude, coordinates.Address, coordinates.Region);
            // assert
            Assert.IsNotNull(instance);
            Assert.AreEqual(coordinates.Latitude, instance.Latitude);
            Assert.AreEqual(coordinates.Longitude, instance.Longitude);
            Assert.AreEqual(coordinates.Address, instance.Address);
            Assert.AreEqual(coordinates.Region, instance.Region);
        }
        [TestMethod]
        [DynamicData(nameof(TestUtilities.IncorrectCoordinates), typeof(TestUtilities), DynamicDataSourceType.Property)]
        public void Coordinates_InvalidCoordinates_ShouldThrowException(double latitude, double longitude, string address, string region)
        {
            // arrange & act & assert
            Assert.ThrowsException<ArgumentException>(() => new Coordinates(latitude, longitude, address, region));
        }
    }
}
