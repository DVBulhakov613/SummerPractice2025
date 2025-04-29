using Microsoft.VisualStudio.TestTools.UnitTesting;
using Class_Lib;
using Class_Lib.Backend.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OOP_CourseProject_TestProject
{
    [TestClass]
    public class EmployeeInheritanceTests
    {
        private DbContextOptions<AppDbContext> _dbContextOptions;

        [TestInitialize]
        public void TestInitialize()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;
        }

        [TestMethod]
        public void ManagerDataPersistsAndInheritsFromEmployee()
        {
            // Arrange
            var workplace = new Warehouse(new Coordinates(50.45, 30.52, "Kyiv", "Ukraine"), 0, false);
            var manager = new Manager("John", "Doe", "123456789", "john.doe@example.com", "Менеджер", workplace)
            {
                ManagedLocations = new List<BaseLocation>{ workplace, new Warehouse(new Coordinates(50, 30, "Kyiv", "Ukraine"), 0, true), new Warehouse(new Coordinates(50.4, 30.5, "Kyiv", "Ukraine"), 50, false) }
            };

            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Employees.Add(manager);
                context.SaveChanges();
            }

            // Act
            Manager retrievedManager;
            using (var context = new AppDbContext(_dbContextOptions))
            {
                retrievedManager = context.Employees
                    .Include(m =>(m as Manager).ManagedLocations)
                    .FirstOrDefault(e => e.ID == manager.ID) as Manager;
            }

            // Assert
            Assert.IsNotNull(retrievedManager);
            Assert.AreEqual(manager.FirstName, retrievedManager.FirstName);
            Assert.AreEqual(manager.Position, retrievedManager.Position);
            Assert.AreEqual(manager.ManagedLocations.Count, retrievedManager.ManagedLocations.Count);
            Assert.IsInstanceOfType(retrievedManager, typeof(Employee));
        }

        [TestMethod]
        public void AdministratorDataPersistsAndInheritsFromManager()
        {
            // Arrange
            var workplace = new Warehouse(new Coordinates(50.45, 30.52, "Kyiv", "Ukraine"), 0, false);
            var administrator = new Administrator("Jane", "Smith", "987654321", "jane.smith@example.com", "Системний Адміністратор", workplace);

            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Employees.Add(administrator);
                context.SaveChanges();
            }

            // Act
            Administrator retrievedAdministrator;
            using (var context = new AppDbContext(_dbContextOptions))
            {
                retrievedAdministrator = context.Employees.Find(administrator.ID) as Administrator;
            }

            // Assert
            Assert.IsNotNull(retrievedAdministrator);
            Assert.AreEqual(administrator.FirstName, retrievedAdministrator.FirstName);
            Assert.AreEqual(administrator.Position, retrievedAdministrator.Position);
            Assert.IsInstanceOfType(retrievedAdministrator, typeof(Manager)); // Check inheritance
            Assert.IsInstanceOfType(retrievedAdministrator, typeof(Employee)); // Check inheritance
        }
    }
}
