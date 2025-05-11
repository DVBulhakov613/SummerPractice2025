using Class_Lib;
using Microsoft.EntityFrameworkCore;
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
    //[TestClass]
    //public class QueryBuilderServiceTests
    //{
    //    //private QueryBuilderService<Employee> _queryBuilder;
    //    //private AppDbContext _context;

    //    //[TestInitialize]
    //    //public void Setup()
    //    //{
    //    //    var options = new DbContextOptionsBuilder<AppDbContext>()
    //    //        .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
    //    //        .Options;

    //    //    _context = new AppDbContext(options);
    //    //    _queryBuilder = new QueryBuilderService<Employee>(_context.Employees);
    //    //}

    //    //[TestCleanup]
    //    //public void Cleanup()
    //    //{
    //    //    _context.Dispose();
    //    //}


    //    //[TestMethod]
    //    //public async Task QueryBuilderService_ShouldFilterDataCorrectly()
    //    //{
    //    //    _context.Employees.AddRange(
    //    //        new Employee { FirstName = "John", Surname = "Doe", Position = "Менеджер", PhoneNumber = "+123456789123", Email = "example@example.com" },
    //    //        new Employee { FirstName = "Jane", Surname = "Smith", Position = "Працівник", PhoneNumber = "+123456789123", Email = "example@example.com" }
    //    //    );
    //    //    await _context.SaveChangesAsync();

    //    //    var queryBuilder = new QueryBuilderService<Employee>(_context.Employees);

    //    //    // Act
    //    //    var result = await queryBuilder
    //    //        .Where(e => e.Position == "Менеджер")
    //    //        .ExecuteAsync();

    //    //    // Assert
    //    //    Assert.AreEqual(1, result.Count);
    //    //    Assert.AreEqual("John", result[0].FirstName);
    //    //}

    //    //[TestMethod]
    //    //public async Task QueryBuilderService_ShouldOrderDataCorrectly()
    //    //{
    //    //    _context.Employees.AddRange(
    //    //        new Employee { FirstName = "John", Surname = "Doe", Position = "Працівник", PhoneNumber = "+123456789123", Email = "example@example.com" },
    //    //        new Employee { FirstName = "Jane", Surname = "Smith", Position = "Працівник", PhoneNumber = "+123456789123", Email = "example@example.com" },
    //    //        new Employee { FirstName = "Alice", Surname = "Brown", Position = "Працівник", PhoneNumber = "+123456789123", Email = "example@example.com" }
    //    //    );
    //    //    await _context.SaveChangesAsync();

    //    //    var queryBuilder = new QueryBuilderService<Employee>(_context.Employees);

    //    //    // Act
    //    //    var ascendingResult = await queryBuilder
    //    //        .OrderBy(e => e.FirstName)
    //    //        .ExecuteAsync();

    //    //    var descendingResult = await queryBuilder
    //    //        .OrderByDescending(e => e.FirstName)
    //    //        .ExecuteAsync();

    //    //    // Assert
    //    //    Assert.AreEqual("Alice", ascendingResult[0].FirstName);
    //    //    Assert.AreEqual("John", descendingResult[0].FirstName);
    //    //}

    //    //[TestMethod]
    //    //public async Task QueryBuilderService_ShouldIncludeNavigationProperties()
    //    //{
    //    //    // Arrange
    //    //    var location = new Warehouse(new Coordinates(20, 20, "Kyiv", "Ukraine"), 0, false);
    //    //    _context.Locations.Add(location);
    //    //    _context.Employees.Add(new Employee { FirstName = "John", Surname = "Doe", Position = "Працівник", Workplace = location, PhoneNumber = "+123456789123", Email = "example@example.com" });
    //    //    await _context.SaveChangesAsync();

    //    //    var queryBuilder = new QueryBuilderService<Employee>(_context.Employees);

    //    //    // Act
    //    //    var result = await queryBuilder
    //    //        .Include(e => e.Workplace)
    //    //        .ExecuteAsync();

    //    //    // Assert
    //    //    Assert.AreEqual(1, result.Count);
    //    //    Assert.IsNotNull(result[0].Workplace);
    //    //    Assert.AreEqual("Warehouse", result[0].Workplace.LocationType);
    //    //}

    //    //[TestMethod]
    //    //public async Task QueryBuilderService_ShouldChainMethodsCorrectly()
    //    //{
    //    //    var location = new Warehouse(new Coordinates(20, 20, "Kyiv", "Ukraine"), 0, false);
    //    //    _context.Locations.Add(location);
    //    //    _context.Employees.AddRange(
    //    //        new Employee { FirstName = "John", Surname = "Doe", Position = "Менеджер", Workplace = location, PhoneNumber="+123456789123", Email = "example@example.com" },
    //    //        new Employee { FirstName = "Jane", Surname = "Smith", Position = "Працівник", Workplace = location, PhoneNumber = "+123456789123", Email = "example@example.com" }
    //    //    );
    //    //    await _context.SaveChangesAsync();

    //    //    var queryBuilder = new QueryBuilderService<Employee>(_context.Employees);

    //    //    // Act
    //    //    var result = await queryBuilder
    //    //        .Where(e => e.Position == "Менеджер")
    //    //        .Include(e => e.Workplace)
    //    //        .OrderBy(e => e.FirstName)
    //    //        .ExecuteAsync();

    //    //    // Assert
    //    //    Assert.AreEqual(1, result.Count);
    //    //    Assert.AreEqual("John", result[0].FirstName);
    //    //    Assert.IsNotNull(result[0].Workplace);
    //    //    Assert.AreEqual("Warehouse", result[0].Workplace.LocationType);
    //    //}

    //    //[TestMethod]
    //    //public async Task QueryBuilderService_ShouldReturnEmptyList_WhenNoDataMatchesFilter()
    //    //{
    //    //    // Arrange
    //    //    _context.Employees.AddRange(
    //    //        new Employee { FirstName = "John", Surname = "Doe", Position = "Не Працівник", PhoneNumber = "+123456789123", Email = "example@example.com" },
    //    //        new Employee { FirstName = "Jane", Surname = "Smith", Position = "Також не Працівник", PhoneNumber = "+123456789123", Email = "example@example.com" }
    //    //    );
    //    //    await _context.SaveChangesAsync();

    //    //    // Act
    //    //    var result = await _queryBuilder
    //    //        .Where(e => e.Position == "Працівник")
    //    //        .ExecuteAsync();

    //    //    // Assert
    //    //    Assert.AreEqual(0, result.Count);
    //    //}


    //    //[TestMethod]
    //    //public async Task QueryBuilderService_ShouldReturnEmptyList_WhenOrderingEmptyDataset()
    //    //{
    //    //    // Act
    //    //    var result = await _queryBuilder
    //    //        .OrderBy(e => e.FirstName)
    //    //        .ExecuteAsync();

    //    //    // Assert
    //    //    Assert.AreEqual(0, result.Count);
    //    //}

    //    //[TestMethod]
    //    //public async Task QueryBuilderService_ShouldHandleNullNavigationProperties()
    //    //{
    //    //    // Arrange
    //    //    _context.Employees.Add(new Employee { FirstName = "John", Surname = "Smith", Position = "Працівник", Workplace = null, PhoneNumber = "+123456789123", Email = "example@example.com" });
    //    //    await _context.SaveChangesAsync();

    //    //    // Act
    //    //    var result = await _queryBuilder
    //    //        .Include(e => e.Workplace)
    //    //        .ExecuteAsync();

    //    //    // Assert
    //    //    Assert.AreEqual(1, result.Count);
    //    //    Assert.IsNull(result[0].Workplace);
    //    //}



    //}

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Class_Lib.Backend.Database;
    using Class_Lib.Database.Repositories;
    using OOP_CourseProject.Controls;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System;
    using GalaSoft.MvvmLight.Command;
    using System.Diagnostics;
    using OOP_CourseProject_TestProject.Class_tests;

    [TestClass]
    public class QueryBuilderTests : TestTemplate
    {
        private EmployeeRepository _repository;
        private PostalOffice _workplace;

        [TestInitialize]
        public void Setup()
        {
            base.Setup();

            _workplace = new PostalOffice(new Coordinates(50, 50, "address", "region"), 50, false, false, false);
            _context.PostalOffices.Add(_workplace);

            _context.Employees.AddRange(
                new Employee("Joe", "Jones", "+123456789012", "example@example.com", "Менеджер", _workplace),
                new Employee("Joe", "Smith", "+123456789012", "example@example.com", "Працівник", _workplace),
                new Employee("Jane", "White", "+123456789012", "example@example.com", "Працівник", _workplace),
                new Employee("John", "Jones", "+123456789012", "example@example.com", "Працівник", _workplace),
                new Employee("Alice", "Jenkins", "+123456789012", "example@example.com", "Працівник", _workplace),
                new Employee("Peter", "Brown", "+123456789012", "example@example.com", "Працівник", _workplace)
            );

            _repository = new EmployeeRepository(_context, _adminUser);

            _context.SaveChanges();
        }

        [TestCleanup]
        public void Clear() => base.Cleanup();

        [TestMethod]
        public async Task QueryBuilder_EqualAndStartsWithFilters_ReturnsCorrectEmployees()
        {
            // Arrange
            var queryBuilderVM = new QueryBuilderViewModel<Employee>();

            // simulate user input for FirstName == "Joe"
            queryBuilderVM.SelectedField = "FirstName";
            queryBuilderVM.SelectedOperator = "==";
            queryBuilderVM.ValueInput = "Joe";
            queryBuilderVM.AddConditionCommand.Execute(null);

            // simulate user input for Surname StartsWith "J"
            queryBuilderVM.SelectedField = "Surname";
            queryBuilderVM.SelectedOperator = "StartsWith";
            queryBuilderVM.ValueInput = "J";
            queryBuilderVM.AddConditionCommand.Execute(null);

            //List<Expression<Func<Employee, bool>>> submittedConditions = null;
            //queryBuilderVM.QuerySubmitted += (conditions) => submittedConditions = conditions;

            // Act
            queryBuilderVM.SubmitQueryCommand.Execute(null);

            var service = new QueryBuilderService<Employee>(_adminUser, _context.Employees); // .AsQueryable()
            service.AddConditions(queryBuilderVM.Conditions.ToList());

            var results = await _repository.GetByCriteriaAsync(service.GetPredicate());

            // Assert
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("Joe", results.First().FirstName);
            Assert.AreEqual("Jones", results.First().Surname);
        }

        [TestMethod]
        public async Task QueryBuilder_OnlyFirstNameJoe_ReturnsTwoEmployees()
        {
            // Arrange
            var condition = new QueryCondition { Field = "FirstName", Operator = "==", Value = "Joe" };
            var service = new QueryBuilderService<Employee>(_adminUser, _context.Employees); // AsQueryable()
            service.AddCondition(condition);

            // Act
            var results = await _repository.GetByCriteriaAsync(service.GetPredicate());

            // Assert
            Assert.AreEqual(2, results.Count());
            Assert.IsTrue(results.All(e => e.FirstName == "Joe"));
        }

        [TestMethod]
        public async Task QueryBuilder_SurnameStartsWithJ_ReturnsThreeEmployees()
        {
            // Arrange
            var service = new QueryBuilderService<Employee>(_adminUser, _context.Employees); // AsQueryable()
            service.AddCondition(new QueryCondition{ Field = "Surname", Operator = "StartsWith", Value = "J" });

            // Act
            var results = await _repository.GetByCriteriaAsync(service.GetPredicate());

            // Assert
            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.All(e => e.Surname.StartsWith("J")));
        }

        [TestMethod]
        public async Task QueryBuilderService_ShouldFilterUsingNestedProperty()
        {
            // Arrange
            var kyivOffice = new PostalOffice(new Coordinates(20, 20, "Kyiv", "Ukraine"), 50, false, false, false);
            var lvivOffice = new PostalOffice(new Coordinates(10, 10, "Lviv", "Ukraine"), 50, false, false, false);
            _context.Locations.AddRange(kyivOffice, lvivOffice);

            _context.Employees.AddRange(
                new Employee("John", "Doe", "+123456789", "example@example.com",  "Працівник", kyivOffice),
                new Employee ("Jane", "Smith", "+123456789", "example@example.com", "Працівник", lvivOffice)
            );
            await _context.SaveChangesAsync();

            var queryBuilder = new QueryBuilderService<Employee>(_adminUser, _context.Employees);

            var filter = QueryBuilderService<Employee>.BuildFilter<Employee>("Workplace.ID", "==", "2");

            var results = await _repository.GetByCriteriaAsync(filter);

            // Assert
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("John", results.First().FirstName);
        }
    }
}
