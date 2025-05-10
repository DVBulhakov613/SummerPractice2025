using Class_Lib;
using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Person_related.Methods;
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
    public class UserTests : TestTemplate
    {
        private UserMethods _userMethods;
        private UserRepository _userRepository;

        [TestInitialize]
        public void Initialize()
        {
            base.Setup();

            _userMethods = _provider.GetRequiredService<UserMethods>();
            _userRepository = _provider.GetRequiredService<UserRepository>();
        }

        [TestCleanup]
        public void Clear() => base.Cleanup();

        #region Create
        [TestMethod]
        public async Task AddAsync()
        {
            // Arrange
            Employee dummy = new Employee("dummy", "dummy", "+000000000000", "example@example.com", "Працівник", null)
            {
                Role = new Role { ID = 999999, Name = "TestRole" }
            };
            User newUser = new("username", PasswordHelper.HashPassword("password"), "TestRole", dummy);

            // Act
            await _userMethods.AddAsync(_adminUser, newUser);

        }

        #endregion


        #region Read
        [TestMethod]
        public async Task GetUserAsync()
        {
            // Arrange
            string username = "user";
            string password = PasswordHelper.HashPassword("password");
            User user = new(username, password, "TestRole", new Employee("dummy", "dummy", "+000000000000", "example@example.com", "Працівник", null) );
            await _userMethods.AddAsync(_adminUser, user);

            // Act
            var retrievedUser = await _userMethods.GetByCustomCriteriaAsync(_adminUser, u => u.Username == username);

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(username, retrievedUser.First().Username);
        }


        #endregion


        #region Update
        [TestMethod]
        public async Task UpdateUserAsync() // i should really make a function to change the password in User
        {
            // Arrange
            string username = "user";
            string password = PasswordHelper.HashPassword("password");
            string newPassword = PasswordHelper.HashPassword("newPassword");
            await _userMethods.AddAsync(_adminUser, new(username, password, "TestRole", new Employee("dummy", "dummy", "+000000000000", "example@example.com", "Працівник", null)));

            // Act
            var retreivedUsers = await _userRepository.Query()
                .Where(u => u.Username == username)
                .ExecuteAsync();
            var retreivedUser = retreivedUsers.First();
            retreivedUser.PasswordHash = newPassword;
            retreivedUser.Role = "OtherRole";
            retreivedUser.Employee.FirstName = "mega";
            await _userRepository.UpdateAsync(retreivedUser);


            // Assert
            retreivedUsers = await _userRepository.Query()
                .Where(u => u.Username == username)
                .ExecuteAsync();
            retreivedUser = retreivedUsers.First();

            Assert.AreEqual(newPassword, retreivedUser.PasswordHash);
            Assert.AreEqual("OtherRole", retreivedUser.Role);
            Assert.AreEqual("mega", retreivedUser.Employee.FirstName);
            Assert.AreEqual(1, retreivedUsers.Count);
        }

        #endregion


        #region Delete
        [TestMethod]
        public async Task DeleteUserAsync()
        {
            // Arrange
            string username = "user";
            string password = PasswordHelper.HashPassword("password");
            await _userMethods.AddAsync(_adminUser, new(username, password, "TestRole", new Employee("dummy", "dummy", "+000000000000", "example@example.com", "Працівник", null)));

            // Act
            await _userMethods.DeleteAsync(_adminUser, "user");

            // Assert
            var retrievedUser = await _userMethods.GetByCustomCriteriaAsync(_adminUser, u => u.Username == username);
            Assert.IsFalse(retrievedUser.Any());
        }

        [TestMethod]
        public async Task DeleteUserAsync_ShouldThrowException_WhenNoPermission()
        {
            // Arrange
            string username = "user";
            string password = PasswordHelper.HashPassword("password");
            await _userMethods.AddAsync(_adminUser, new(username, password, "TestRole", new Employee("dummy", "dummy", "+000000000000", "example@example.com", "Працівник", null)));

            // Act + Assert
            await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(async () => await _userMethods.DeleteAsync(_unauth, "user"));
        }
        #endregion
    }
}
