using Class_Lib;
using Class_Lib.Backend.Package_related;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject_TestProject.Class_tests
{
    [TestClass]
    public class WarehouseTests
    {
        [TestClass]
        public class BaseLocationTests
        {
            [TestMethod]
            public void Constructor_ValidGeoData_InitializesProperties()
            {
                var coords = new Coordinates(10, 20, "Test Address", "Test Region");
                var location = new Warehouse(coords, 10, false);

                Assert.AreEqual(coords, location.GeoData);
                Assert.IsNotNull(location.Staff);
            }

            [TestMethod]
            [ExpectedException(typeof(AggregateException))]
            public void Constructor_NullGeoData_ThrowsException()
            {
                var location = new Warehouse(null, 10, false);
            }

            [TestMethod]
            public void AddEmployee_ValidEmployee_AddsToStaff()
            {
                var coords = new Coordinates(10, 20, "Test Address", "Test Region");
                var location = new Warehouse(coords, 10, false);
                var employee = new Employee("John", "Doe", "+123456789", "john@doe.com", location);

                location.AddEmployee(employee);

                Assert.IsTrue(location.Staff.Contains(employee));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void AddEmployee_DuplicateEmployee_ThrowsException()
            {
                var coords = new Coordinates(10, 20, "Test Address", "Test Region");
                var location = new Warehouse(coords, 10, false);
                var employee = new Employee("John", "Doe", "+123456789", "john@doe.com", location);

                location.AddEmployee(employee);
                location.AddEmployee(employee); // Should throw
            }

            [TestMethod]
            public void RemoveEmployee_ValidEmployee_RemovesFromStaff()
            {
                var coords = new Coordinates(10, 20, "Test Address", "Test Region");
                var location = new Warehouse(coords, 10, false);
                Employee employee = new Employee("John", "Doe", "+123456789", "john@doe.com", location);

                location.AddEmployee(employee);
                location.RemoveEmployee(employee);

                Assert.IsFalse(location.Staff.Contains(employee));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void RemoveEmployee_NotInStaff_ThrowsException()
            {
                var coords = new Coordinates(10, 20, "Test Address", "Test Region");
                var location = new Warehouse(coords, 10, false);
                var employee = new Employee("John", "Doe", "+123456789", "john@doe.com", location);

                location.RemoveEmployee(employee); // Should throw
            }

            [TestMethod]
            public void UpdateGeoData_ChangesGeoData()
            {
                var coords = new Coordinates(10, 20, "Test Address", "Test Region");
                var newCoords = new Coordinates(30, 40, "New Address", "New Region");
                var location = new Warehouse(coords, 10, false);

                location.UpdateGeoData(newCoords);

                Assert.AreEqual(newCoords, location.GeoData);
            }

            [TestMethod]
            public void StorePackage_AddsPackage_WhenNotFull()
            {
                var warehouse = new Warehouse(new Coordinates(1, 2, "A", "B"), 2, false);
                var package = new Package(1, 1, 1, 1, null, null, warehouse, warehouse, PackageType.Standard);

                warehouse.StorePackage(package);

                Assert.IsTrue(warehouse.StoredPackages.Contains(package));
                Assert.IsFalse(warehouse.IsFull);
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void StorePackage_Throws_WhenFull()
            {
                var warehouse = new Warehouse(new Coordinates(1, 2, "A", "B"), 1, false);
                var package1 = new Package(1, 1, 1, 1, null, null, warehouse, warehouse, PackageType.Standard);
                var package2 = new Package(2, 2, 2, 2, null, null, warehouse, warehouse, PackageType.Standard);

                warehouse.StorePackage(package1);
                warehouse.StorePackage(package2); // Should throw
            }

            [TestMethod]
            public void RemovePackage_RemovesPackage_WhenExists()
            {
                var warehouse = new Warehouse(new Coordinates(1, 2, "A", "B"), 2, false);
                var package = new Package(1, 1, 1, 1, null, null, warehouse, warehouse, PackageType.Standard);

                warehouse.StorePackage(package);
                warehouse.RemovePackage(package);

                Assert.IsFalse(warehouse.StoredPackages.Contains(package));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void RemovePackage_Throws_WhenNotExists()
            {
                var warehouse = new Warehouse(new Coordinates(1, 2, "A", "B"), 2, false);
                var package = new Package(1, 1, 1, 1, null, null, warehouse, warehouse, PackageType.Standard);

                warehouse.RemovePackage(package); // Should throw
            }

            [TestMethod]
            public void ClearStorage_RemovesAllPackages()
            {
                var warehouse = new Warehouse(new Coordinates(1, 2, "A", "B"), 2, false);
                var package1 = new Package(1, 1, 1, 1, null, null, warehouse, warehouse, PackageType.Standard);
                var package2 = new Package(2, 2, 2, 2, null, null, warehouse, warehouse, PackageType.Standard);

                warehouse.StorePackage(package1);
                warehouse.StorePackage(package2);

                warehouse.ClearStorage();

                Assert.AreEqual(0, warehouse.StoredPackages.Count);
            }

            [TestMethod]
            public void SendPackage_MovesPackageToOtherWarehouse()
            {
                var warehouse1 = new Warehouse(new Coordinates(1, 2, "A", "B"), 2, false);
                var warehouse2 = new PostalOffice(new Coordinates(3, 4, "C", "D"), 2, false, false, false);
                var package = new Package(1, 1, 1, 1, null, null, warehouse1, warehouse2, PackageType.Standard);

                warehouse1.StorePackage(package);
                warehouse1.SendPackage(package, warehouse1 as PostalOffice, warehouse2);

                Assert.IsFalse(warehouse1.StoredPackages.Contains(package));
                Assert.IsTrue(warehouse2.StoredPackages.Contains(package));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void SendPackage_Throws_WhenPackageNotInWarehouse()
            {
                var warehouse1 = new Warehouse(new Coordinates(1, 2, "A", "B"), 2, false);
                var warehouse2 = new PostalOffice(new Coordinates(3, 4, "C", "D"), 2, false, false, false);
                var package = new Package(1, 1, 1, 1, null, null, warehouse1, warehouse2, PackageType.Standard);

                warehouse1.SendPackage(package, warehouse1 as PostalOffice, warehouse2); // Should throw
            }
        }
    }

}

