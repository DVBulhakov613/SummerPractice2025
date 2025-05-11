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
            Employee dummy = new Employee("dummy", "dummy", "+000000000000", "example@example.com", "Працівник", null);

            User newUser = new("username", PasswordHelper.HashPassword("password"), new Role { Name="TestRole" }, dummy);

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
            User user = new(username, password, new Role {Name = "TestRole" }, new Employee("dummy", "dummy", "+000000000000", "example@example.com", "Працівник", null) );
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
        public async Task ChangePasswordAndRoleAsync()
        {
            // Arrange
            var _roleRepository = _provider.GetRequiredService<RoleRepository>();
            Role role = new Role { Name = "Role1" };
            await _roleRepository.AddAsync(role);
            role = new Role { Name = "Role2" };
            await _roleRepository.AddAsync(role);
            string username = "user";
            string oldPassword = PasswordHelper.HashPassword("password");
            string newPassword = "newPassword"; // un-hashed here
            int newRoleId = 2;

            // Create role and user
            var initialRole = await _roleRepository.GetByIdAsync(1);
            var employee = new Employee("mega", "dummy", "+000000000000", "example@example.com", "Працівник", null);
            var user = new User(username, oldPassword, initialRole, employee);

            await _userMethods.AddAsync(_adminUser, user);

            var retrieved = await _userRepository.GetByCriteriaAsync(u => u.Username == username);
            var existingUser = retrieved.First();

            // Act: change password
            await _userMethods.ChangePasswordAsync(_adminUser, (uint)existingUser.PersonID, newPassword);

            // Act: change role
            var newRole = await _roleRepository.GetByIdAsync((uint)newRoleId);
            existingUser.Role = newRole;
            await _userMethods.UpdateAsync(_adminUser, existingUser);

            // Assert
            retrieved = await _userRepository.GetByCriteriaAsync(u => u.Username == username);
            var updatedUser = retrieved.First();

            Assert.IsTrue(PasswordHelper.VerifyPassword(newPassword, updatedUser.PasswordHash));
            Assert.AreEqual("Role2", updatedUser.Role.Name);
            Assert.AreEqual("mega", updatedUser.Employee.FirstName);
            Assert.AreEqual(1, retrieved.Count());
        }


        #endregion


        #region Delete
        [TestMethod]
        public async Task DeleteUserAsync()
        {
            // Arrange
            string username = "user";
            string password = PasswordHelper.HashPassword("password");
            await _userMethods.AddAsync(_adminUser, new(username, password, new Role { Name = "TestRole" }, new Employee("dummy", "dummy", "+000000000000", "example@example.com", "Працівник", null)));

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
            await _userMethods.AddAsync(_adminUser, new(username, password, new Role {Name = "TestRole" }, new Employee("dummy", "dummy", "+000000000000", "example@example.com", "Працівник", null)));

            // Act + Assert
            await Assert.ThrowsExceptionAsync<UnauthorizedAccessException>(async () => await _userMethods.DeleteAsync(_unauth, "user"));
        }
        #endregion
    }
}
