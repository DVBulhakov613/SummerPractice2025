using Class_Lib;
using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
namespace OOP_CourseProject_TestProject
{
    [TestClass]
    public class ValidEmployeeTests
    {
        private AppDbContext _context;
        private EmployeeRepository _repository;
        private PostalOffice _workplace;
        private Employee _employee1;
        private Employee _employee2;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _repository = new EmployeeRepository(_context);

            _workplace = (PostalOffice)TestUtilities.CorrectPostalOffice.First()[0];
            _employee1 = new Employee("John", "Doe", "+123456789", "Clerk", _workplace);
            _employee2 = new Employee("Jane", "Smith", "+987654321", "Manager", _workplace);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetEmployeesByIdAsync()
        {
            // arrange
            await _repository.AddAsync(_employee1);
            await _repository.AddAsync(_employee2);

            // act
            var employee1id = await _repository.GetEmployeesByIDAsync(_employee1.ID);
            var employee2id = await _repository.GetEmployeesByIDAsync(_employee2.ID);

            // assert
            Assert.AreEqual((uint)1, employee1id.First().ID);
            Assert.AreEqual((uint)2, employee2id.First().ID);
        }

        [TestMethod]
        public async Task GetEmployeesByWorkplaceIdAsync()
        {
            // arrange
            await _repository.AddAsync(_employee1);
            await _repository.AddAsync(_employee2);

            // act
            var employees = await _repository.GetEmployeesByWorkplaceIdAsync(_workplace.ID);

            // assert
            Assert.AreEqual(2, employees.Count());
            Assert.IsTrue(employees.All(e => e.Workplace.ID == _workplace.ID));
        }

        [TestMethod]
        [DynamicData(nameof(TestUtilities.CorrectPersonNames), typeof(TestUtilities), DynamicDataSourceType.Property)]
        public async Task GetEmployeesByFirstNameAsync(string firstName)
        {
            // arrange
            _employee1.FirstName = firstName;
            await _repository.AddAsync(_employee1);
            await _repository.AddAsync(_employee2);

            // act
            var employees = await _repository.GetEmployeesByFirstNameAsync(firstName);

            // assert
            Assert.AreEqual(1, employees.Count());
            Assert.AreEqual(firstName, employees.First().FirstName);
        }

        [TestMethod]
        public async Task GetEmployeesByLastNameAsync()
        {
            // arrange
            _employee1.Surname = "Doe";
            _employee2.Surname = "Doe";
            await _repository.AddAsync(_employee1);
            await _repository.AddAsync(_employee2);

            // act
            var employees = await _repository.GetEmployeesByLastNameAsync("Doe");

            // assert
            Assert.AreEqual(2, employees.Count());
            Assert.IsTrue(employees.All(e => e.Surname == "Doe"));
        }

        [TestMethod]
        public async Task GetEmployeesByFullNameAsync()
        {
            // arrange
            _employee1.FirstName = "John";
            _employee1.Surname = "Doe";
            await _repository.AddAsync(_employee1);
            await _repository.AddAsync(_employee2);

            // act
            var employees = await _repository.GetEmployeesByFullNameAsync("John Doe");

            // assert
            Assert.AreEqual(1, employees.Count());
            Assert.AreEqual("John Doe", employees.First().FullName);
        }
    }
}