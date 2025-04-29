using Class_Lib;
using Class_Lib.Backend.Database;
using Microsoft.EntityFrameworkCore;
using Moq;
using OOP_CourseProject;
using OOP_CourseProject.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject_TestProject
{
    [TestClass]
    public class QueryBuilderServiceTests
    {
        private QueryBuilderService<Employee> _queryBuilder;
        private AppDbContext _context;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _queryBuilder = new QueryBuilderService<Employee>(_context.Employees);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }


        [TestMethod]
        public async Task QueryBuilderService_ShouldFilterDataCorrectly()
        {
            _context.Employees.AddRange(
                new Employee { FirstName = "John", Surname = "Doe", Position = "Менеджер", PhoneNumber = "+123456789123" },
                new Employee { FirstName = "Jane", Surname = "Smith", Position = "Працівник", PhoneNumber = "+123456789123" }
            );
            await _context.SaveChangesAsync();

            var queryBuilder = new QueryBuilderService<Employee>(_context.Employees);

            // Act
            var result = await queryBuilder
                .Where(e => e.Position == "Менеджер")
                .ExecuteAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("John", result[0].FirstName);
        }

        [TestMethod]
        public async Task QueryBuilderService_ShouldOrderDataCorrectly()
        {
            _context.Employees.AddRange(
                new Employee { FirstName = "John", Surname = "Doe", Position = "Працівник", PhoneNumber = "+123456789123" },
                new Employee { FirstName = "Jane", Surname = "Smith", Position = "Працівник", PhoneNumber = "+123456789123" },
                new Employee { FirstName = "Alice", Surname = "Brown", Position = "Працівник", PhoneNumber = "+123456789123" }
            );
            await _context.SaveChangesAsync();

            var queryBuilder = new QueryBuilderService<Employee>(_context.Employees);

            // Act
            var ascendingResult = await queryBuilder
                .OrderBy(e => e.FirstName)
                .ExecuteAsync();

            var descendingResult = await queryBuilder
                .OrderByDescending(e => e.FirstName)
                .ExecuteAsync();

            // Assert
            Assert.AreEqual("Alice", ascendingResult[0].FirstName);
            Assert.AreEqual("John", descendingResult[0].FirstName);
        }

        [TestMethod]
        public async Task QueryBuilderService_ShouldIncludeNavigationProperties()
        {
            // Arrange
            var location = new Warehouse(new Coordinates(20, 20, "Kyiv", "Ukraine"), 0, false);
            _context.Locations.Add(location);
            _context.Employees.Add(new Employee { FirstName = "John", Surname = "Doe", Position = "Працівник", Workplace = location, PhoneNumber = "+123456789123" });
            await _context.SaveChangesAsync();

            var queryBuilder = new QueryBuilderService<Employee>(_context.Employees);

            // Act
            var result = await queryBuilder
                .Include(e => e.Workplace)
                .ExecuteAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0].Workplace);
            Assert.AreEqual("Warehouse", result[0].Workplace.LocationType);
        }

        [TestMethod]
        public async Task QueryBuilderService_ShouldChainMethodsCorrectly()
        {
            var location = new Warehouse(new Coordinates(20, 20, "Kyiv", "Ukraine"), 0, false);
            _context.Locations.Add(location);
            _context.Employees.AddRange(
                new Employee { FirstName = "John", Surname = "Doe", Position = "Менеджер", Workplace = location, PhoneNumber="+123456789123" },
                new Employee { FirstName = "Jane", Surname = "Smith", Position = "Працівник", Workplace = location, PhoneNumber = "+123456789123" }
            );
            await _context.SaveChangesAsync();

            var queryBuilder = new QueryBuilderService<Employee>(_context.Employees);

            // Act
            var result = await queryBuilder
                .Where(e => e.Position == "Менеджер")
                .Include(e => e.Workplace)
                .OrderBy(e => e.FirstName)
                .ExecuteAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("John", result[0].FirstName);
            Assert.IsNotNull(result[0].Workplace);
            Assert.AreEqual("Warehouse", result[0].Workplace.LocationType);
        }

        [TestMethod]
        public async Task QueryBuilderService_ShouldReturnEmptyList_WhenNoDataMatchesFilter()
        {
            // Arrange
            _context.Employees.AddRange(
                new Employee { FirstName = "John", Surname = "Doe", Position = "Не Працівник", PhoneNumber = "+123456789123" },
                new Employee { FirstName = "Jane", Surname = "Smith", Position = "Також не Працівник", PhoneNumber = "+123456789123" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _queryBuilder
                .Where(e => e.Position == "Працівник")
                .ExecuteAsync();

            // Assert
            Assert.AreEqual(0, result.Count);
        }


        [TestMethod]
        public async Task QueryBuilderService_ShouldReturnEmptyList_WhenOrderingEmptyDataset()
        {
            // Act
            var result = await _queryBuilder
                .OrderBy(e => e.FirstName)
                .ExecuteAsync();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task QueryBuilderService_ShouldHandleNullNavigationProperties()
        {
            // Arrange
            _context.Employees.Add(new Employee { FirstName = "John", Surname = "Smith", Position = "Працівник", Workplace = null, PhoneNumber = "+123456789123" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _queryBuilder
                .Include(e => e.Workplace)
                .ExecuteAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.IsNull(result[0].Workplace);
        }

    }
}
