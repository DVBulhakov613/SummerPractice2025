using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Services;
using Class_Lib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class_Lib.Backend.Person_related;

namespace OOP_CourseProject_TestProject
{
    [TestClass]
    public class UserRepositoryTests
    {
        private AppDbContext _context;
        private UserRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _repository = new UserRepository(_context);
        }

        [TestMethod]
        public async Task AddUser_ShouldPersistToDatabase()
        {
            // Arrange
            var user = new User
            {
                Username = "john.doe",
                PasswordHash = PasswordHelper.HashPassword("securepassword"),
                Role = "Employee"
            };

            // Act
            await _repository.AddAsync(user);
            var retrievedUser = await _repository.GetByUsernameAsync("john.doe");

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(user.Username, retrievedUser.Username);
        }
    }
}
