using Class_Lib;
using Class_Lib.Backend;
using Class_Lib.Backend.Database;
using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace OOP_CourseProject_TestProject.Class_tests
{
    [TestClass]
    public class EmployeeTests : TestTemplate
    {
        private EmployeeRepository _repository;
        private UserRepository _userRepository;
        private EmployeeMethods _employeeMethods;

        //private Employee _managerUser = new("manager", "dummy", "+000000000000", "dummy@dummy.com", 2, null);
        //private Employee _workerUser = new("worker", "dummy", "+000000000000", "dummy@dummy.com", 3, null);
        private Employee _employee1;
        private Employee _employee2;
        private PostalOffice _workplace = new(new Coordinates(49.807647, 36.053660, "вул. Дніпропетровська", "Харківська область, Мерефа"), 1000, false, true, false);


        [TestInitialize]
        public void Initialize()
        {
            base.Setup();
            _repository = _provider.GetRequiredService<EmployeeRepository>();
            _employeeMethods = _provider.GetRequiredService<EmployeeMethods>();
            _userRepository = _provider.GetRequiredService<UserRepository>();

            _employee1 = new("John", "Doe", "+000000000000", "example@example.com", "Працівник", _workplace);
            _employee2 = new("Jane", "Boe", "+000000000000", "example@example.com", "Працівник", _workplace);
        }

        [TestCleanup]
        public void Clear() => base.Cleanup();

        [TestMethod]
        public async Task GetEmployeesByIdAsync()
        {
            // arrange
            await _repository.AddAsync(_employee1);
            await _repository.AddAsync(_employee2);

            // act
            var employee1id = await _repository.GetByIDAsync(_employee1.ID);
            var employee2id = await _repository.GetByIDAsync(_employee2.ID);

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
            var employees = await _repository.GetByWorkplaceIdAsync(_workplace.ID);

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
            var employees = await _repository.GetByFirstNameAsync(firstName);

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
            var employees = await _repository.GetByLastNameAsync("Doe");

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
            var employees = await _repository.GetByFullNameAsync("John Doe");

            // assert
            Assert.AreEqual(1, employees.Count());
            Assert.AreEqual("John Doe", employees.First().FullName);
        }

        // Create / With permission
        [TestMethod]
        public async Task AddEmployeeAsync_ShouldAddEmployee_WhenUserHasPermission()
        {
            // Arrange string name, string surname, string phoneNumber, string? email, string position, BaseLocation workplace
            _adminUser.CachedPermissions.Add((int)AccessService.PermissionKey.CreateEmployee);
            var newEmployee = new Employee("New", "Працівник", "+987654321", "example@email.com", "Працівник", _workplace);

            // Act
            await _employeeMethods.AddAsync(_adminUser, newEmployee);
            var employees = await _repository.GetAllAsync();

            // Assert
            Assert.AreEqual(1, employees.Count());
            Assert.AreEqual("New", employees.First().FirstName);
        }

        // Create / Without permission
        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public async Task AddEmployeeAsync_ShouldThrowException_WhenUserLacksPermission()
        {
            // Arrange
            User user = new User("dummy", PasswordHelper.HashPassword("dummypassword"), new Role { Name = "TestRole" }, new Employee("Новий", "Працівник", "+987654321", "example@email.com", "Працівник", _workplace));
            var newEmployee = new Employee("New", "Працівник", "+987654321", "example@email.com", "Працівник", _workplace);

            // Act
            await _employeeMethods.AddAsync(user, newEmployee);
        }

        [TestMethod]
        public async Task DeleteEmployeeAsync_ShouldDeleteEmployee_WhenUserHasPermission()
        {
            // Arrange
            User user = new User("dummy", PasswordHelper.HashPassword("dummypassword"), new Role { Name = "TestRole" }, new Employee("Новий", "Працівник", "+987654321", "example@email.com", "Працівник", _workplace));
            var newEmployee = new Employee("New", "Працівник", "+987654321", "example@email.com", "Працівник", _workplace);
            newEmployee.User = user;
            await _repository.AddAsync(newEmployee);

            // Act
            await _employeeMethods.DeleteAsync(_adminUser, newEmployee);
            var employees = await _repository.GetAllAsync();

            // Assert
            Assert.AreEqual(0, employees.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task DeleteEmployeeAsync_ShouldThrowException_WhenDeletingLastAdministrator()
        {
            // Arrange
            var roleRepo = _provider.GetRequiredService<RoleRepository>();
            await roleRepo.AddAsync(new Role { ID = 1, Name = "Системний Адміністратор" });

            var admin = new Employee("Admin", "User", "+123456789", "admin@example.com", "Системний Адміністратор", _workplace);
            var adminRole = new User("dummy", PasswordHelper.HashPassword("dummypassword"), new Role { }, admin)
            {
                Role = await roleRepo.GetByIdAsync(1)
            };
            admin.User = adminRole;

            await _employeeMethods.AddAsync(_adminUser, admin);

            // Act + Assert
            await _employeeMethods.DeleteAsync(_adminUser, admin);
        }

        [TestMethod]
        public async Task PromoteEmployeeToManager_ShouldPromoteToManager()
        {
            // Arrange
            var rolerepo = _provider.GetRequiredService<RoleRepository>();
            await rolerepo.AddAsync(new Role { ID = 999999, Name = "TestRole" });
            await rolerepo.AddAsync(new Role { ID = 2, Name = "Менеджер" });

            var employee = new Employee("Name", "Surname", "+123456789", "example@example.com", "Працівник", _workplace);
            User newUser = new User("dummy", PasswordHelper.HashPassword("dummypassword"), new Role { }, employee);
            newUser.Role = await rolerepo.GetByIdAsync(999999);
            employee.User = newUser;

            await _repository.AddAsync(employee);

            var managedLocations = new List<BaseLocation> { _workplace };

            // Act
            await _employeeMethods.PromoteToManagerAsync(_adminUser, employee, managedLocations);
            var manager = await _repository.GetByIdAsync(employee.ID);

            // Assert
            Assert.IsNotNull(manager);
            Assert.AreEqual(employee.FirstName, manager.FirstName);
            Assert.IsNotNull(manager.ManagedLocations);
            Assert.IsNotNull(manager.User);
            Assert.AreEqual(managedLocations.Count, manager.ManagedLocations.Count);
            Assert.AreEqual("Менеджер", manager.User.Role.Name);
            Assert.IsInstanceOfType(manager, typeof(Employee));
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public async Task PromoteToManagerAsync_ShouldThrowException_WhenEmployeeNotFound()
        {
            // Arrange
            _adminUser.CachedPermissions.Add((int)AccessService.PermissionKey.UpdateEmployee);
            var nonExistentEmployee = new Employee("Non", "Existent", "+000000000", "thisdoesnotexist@not.real", "Працівник", _workplace);

            // Act
            await _employeeMethods.PromoteToManagerAsync(_adminUser, nonExistentEmployee, new List<BaseLocation>());
        }

        [TestMethod]
        public async Task PromoteToAdministratorAsync_ShouldPromoteEmployeeToAdministrator()
        {
            // Arrange
            var rolerepo = _provider.GetRequiredService<RoleRepository>();
            await rolerepo.AddAsync(new Role { ID = 1, Name = "Системний Адміністратор" });

            var newEmployee = new Employee("Non", "Existent", "+000000000", "thisdoesnotexist@not.real", "Працівник", _workplace);
            var newUser = new User("dummy", PasswordHelper.HashPassword("dummypassword"), new Role { ID = 9999, Name = "Працівник" }, newEmployee);
            await _repository.AddAsync(newEmployee);

            // Act
            await _employeeMethods.PromoteToAdministratorAsync(_adminUser, newEmployee);
            var administrators = await _repository.GetByCriteriaAsync(e => e.User.Role.Name == "Системний Адміністратор");

            // Assert
            Assert.AreEqual(1, administrators.Count());
        }


        [TestMethod]
        public async Task ManagerData_ShouldPersistCorrectly()
        {
            // Arrange
            var workplace = new Warehouse(new Coordinates(50.45, 30.52, "Kyiv", "Ukraine"), 0, false);
            var manager = new Employee("Manager", "NotUser", "+123456789", "manager@example.com", "Менеджер", workplace);
            var newUser = new User("dummy", PasswordHelper.HashPassword("dummypassword"), new Role { }, manager);
            newUser.Role = new Role { ID = 2, Name = "Менеджер" };
            manager.User = newUser;
            manager.ManagedLocations = new List<BaseLocation>
                    {
                    workplace,
                    new Warehouse(new Coordinates(50, 30, "Kyiv", "Ukraine"), 0, true),
                    new Warehouse(new Coordinates(50.4, 30.5, "Kyiv", "Ukraine"), 50, false)

                    };

            await _repository.AddAsync(manager);

            // Act
            var retrievedManager = await _repository.GetByIdAsync(manager.ID);

            // Assert
            Assert.IsNotNull(retrievedManager);
            Assert.AreEqual(manager.FirstName, retrievedManager.FirstName);
            Assert.AreEqual(manager.User.Role.Name, retrievedManager.User.Role.Name);
            Assert.AreEqual(manager.ManagedLocations.Count, retrievedManager.ManagedLocations.Count);
            Assert.AreEqual("Менеджер", retrievedManager.User.Role.Name);
        }

        //[TestMethod]
        //public async Task AdministratorData_ShouldPersistCorrectly()
        //{
        //    // Arrange
        //    var workplace = new Warehouse(new Coordinates(50.45, 30.52, "Kyiv", "Ukraine"), 0, false);
        //    var administrator = new Employee("Admin", "User", "+123456789", "admin@example.com", "Системний Адміністратор", workplace);

        //    await _repository.AddAsync(administrator);

        //    // Act
        //    var retrievedAdministrator = await _repository.GetByIdAsync(administrator.ID);

        //    // Assert
        //    Assert.IsNotNull(retrievedAdministrator);
        //    Assert.AreEqual(administrator.FirstName, retrievedAdministrator.FirstName);
        //    Assert.AreEqual(administrator.Role.Name, retrievedAdministrator.Role.Name);
        //    Assert.AreEqual("Системний Адміністратор", retrievedAdministrator.Role.Name);
        //}
    }
}
//    [TestClass]
//    public class EmployeeTests
//    {
//        private AppDbContext _context;
//        private EmployeeRepository _repository;
//        private PostalOffice _workplace;
//        private Employee _employee1;
//        private Employee _employee2;
//        private Employee _user;

//        [TestInitialize]
//        public void Setup()
//        {
//            var options = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
//                .Options;

//            _workplace = (PostalOffice)TestUtilities.CorrectPostalOffice.First()[0];
//            _employee1 = new Employee("John", "Doe", "+123456789", "example@email.com", "Працівник", _workplace);
//            _employee2 = new Employee("Jane", "Smith", "+987654321", "example@email.com", "Менеджер", _workplace);

//            _user = TestUtilities.CorrectEmployees.ElementAt(2)[0] as Employee;
//            _context = new AppDbContext(options);
//            _repository = new EmployeeRepository(_context, _user);
//        }

//        [TestCleanup]
//        public void Cleanup()
//        {
//            _context.Dispose();
//        }

//        [TestMethod]
//        public async Task GetEmployeesByIdAsync()
//        {
//            // arrange
//            await _repository.AddAsync(_employee1);
//            await _repository.AddAsync(_employee2);

//            // act
//            var employee1id = await _repository.GetByIDAsync(_employee1.ID);
//            var employee2id = await _repository.GetByIDAsync(_employee2.ID);

//            // assert
//            Assert.AreEqual((uint)1, employee1id.First().ID);
//            Assert.AreEqual((uint)2, employee2id.First().ID);
//        }

//        [TestMethod]
//        public async Task GetEmployeesByWorkplaceIdAsync()
//        {
//            // arrange
//            await _repository.AddAsync(_employee1);
//            await _repository.AddAsync(_employee2);

//            // act
//            var employees = await _repository.GetByWorkplaceIdAsync(_workplace.ID);

//            // assert
//            Assert.AreEqual(2, employees.Count());
//            Assert.IsTrue(employees.All(e => e.Workplace.ID == _workplace.ID));
//        }

//        [TestMethod]
//        [DynamicData(nameof(TestUtilities.CorrectPersonNames), typeof(TestUtilities), DynamicDataSourceType.Property)]
//        public async Task GetEmployeesByFirstNameAsync(string firstName)
//        {
//            // arrange
//            _employee1.FirstName = firstName;
//            await _repository.AddAsync(_employee1);
//            await _repository.AddAsync(_employee2);

//            // act
//            var employees = await _repository.GetByFirstNameAsync(firstName);

//            // assert
//            Assert.AreEqual(1, employees.Count());
//            Assert.AreEqual(firstName, employees.First().FirstName);
//        }

//        [TestMethod]
//        public async Task GetEmployeesByLastNameAsync()
//        {
//            // arrange
//            _employee1.Surname = "Doe";
//            _employee2.Surname = "Doe";
//            await _repository.AddAsync(_employee1);
//            await _repository.AddAsync(_employee2);

//            // act
//            var employees = await _repository.GetByLastNameAsync("Doe");

//            // assert
//            Assert.AreEqual(2, employees.Count());
//            Assert.IsTrue(employees.All(e => e.Surname == "Doe"));
//        }

//        [TestMethod]
//        public async Task GetEmployeesByFullNameAsync()
//        {
//            // arrange
//            _employee1.FirstName = "John";
//            _employee1.Surname = "Doe";
//            await _repository.AddAsync(_employee1);
//            await _repository.AddAsync(_employee2);

//            // act
//            var employees = await _repository.GetByFullNameAsync("John Doe");

//            // assert
//            Assert.AreEqual(1, employees.Count());
//            Assert.AreEqual("John Doe", employees.First().FullName);
//        }

//        [TestMethod]
//        public async Task AddEmployeeAsync_ShouldAddEmployee_WhenUserHasPermission()
//        {
//            // Arrange string name, string surname, string phoneNumber, string? email, string position, BaseLocation workplace
//            var employeeMethods = new EmployeeMethods(_repository);
//            var user = new Employee("Admin", "User", "+123456789", "admin@example.com", "Системний Адміністратор", _workplace);
//            var newEmployee = new Employee("New", "Працівник", "+987654321", "example@email.com", "Працівник", _workplace);

//            // Act
//            await employeeMethods.AddEmployeeAsync(user, newEmployee);
//            var employees = await _repository.GetAllAsync();

//            // Assert
//            Assert.AreEqual(1, employees.Count());
//            Assert.AreEqual("New", employees.First().FirstName);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(UnauthorizedAccessException))]
//        public async Task AddEmployeeAsync_ShouldThrowException_WhenUserLacksPermission()
//        {
//            // Arrange
//            var employeeMethods = new EmployeeMethods(_repository);
//            var user = new Employee("Regular", "User", "+123456789", "example@email.com", "Працівник", _workplace);
//            var newEmployee = new Employee("New", "Працівник", "+987654321", "example@email.com", "Працівник", _workplace);

//            // Act
//            await employeeMethods.AddEmployeeAsync(user, newEmployee);
//        }

//        [TestMethod]
//        public async Task DeleteEmployeeAsync_ShouldDeleteEmployee_WhenUserHasPermission()
//        {
//            // Arrange
//            var employeeMethods = new EmployeeMethods(_repository);
//            var user = new Employee("Admin", "User", "+123456789", "admin@example.com", "Системний Адміністратор", _workplace);
//            await _repository.AddAsync(_employee1);

//            // Act
//            await employeeMethods.DeleteEmployeeAsync(user, _employee1);
//            var employees = await _repository.GetAllAsync();

//            // Assert
//            Assert.AreEqual(0, employees.Count());
//        }

//        [TestMethod]
//        [ExpectedException(typeof(InvalidOperationException))]
//        public async Task DeleteEmployeeAsync_ShouldThrowException_WhenDeletingLastAdministrator()
//        {
//            // Arrange
//            var employeeMethods = new EmployeeMethods(_repository);
//            var user = new Employee("Admin", "User", "+123456789", "admin@example.com", "Системний Адміністратор", _workplace);
//            var admin = new Employee("Admin", "User", "+123456789", "admin@example.com", "Системний Адміністратор", _workplace);
//            await _repository.AddAsync(admin);

//            // Act
//            await employeeMethods.DeleteEmployeeAsync(user, admin);
//        }

//        [TestMethod]
//        public async Task PromoteEmployeeToManager_ShouldPromoteToManager()
//        {
//            // Arrange
//            var user = new Employee("Admin", "User", "+123456789", "admin@example.com", "Системний Адміністратор", _workplace);
//            var employee = new Employee("Name", "Surname", "+123456789", "example@example.com", "Працівник", _workplace);
//            await _repository.AddAsync(user);
//            await _repository.AddAsync(employee);

//            var managedLocations = new List<BaseLocation> { _workplace };

//            // Act
//            var employeeMethods = new EmployeeMethods(_repository);
//            await employeeMethods.PromoteToManagerAsync(user, employee, managedLocations);
//            var manager = await _repository.GetByIdAsync(employee.ID);

//            // Assert
//            Assert.IsNotNull(manager);
//            Assert.AreEqual(employee.FirstName, manager.FirstName);
//            Assert.AreEqual(managedLocations.Count, manager.ManagedLocations.Count);
//            Assert.AreEqual("Менеджер", manager.Position);
//            Assert.IsInstanceOfType(manager, typeof(Employee));
//        }

//        [TestMethod]
//        [ExpectedException(typeof(KeyNotFoundException))]
//        public async Task PromoteToManagerAsync_ShouldThrowException_WhenEmployeeNotFound()
//        {
//            // Arrange
//            var employeeMethods = new EmployeeMethods(_repository);
//            var user = new Employee("Admin", "User", "+123456789", "admin@example.com", "Системний Адміністратор", _workplace);
//            var nonExistentEmployee = new Employee("Non", "Existent", "+000000000", "thisdoesnotexist@not.real", "Працівник", _workplace);

//            // Act
//            await employeeMethods.PromoteToManagerAsync(user, nonExistentEmployee, new List<BaseLocation>());
//        }

//        [TestMethod]
//        public async Task PromoteToAdministratorAsync_ShouldPromoteEmployeeToAdministrator()
//        {
//            // Arrange
//            var employeeMethods = new EmployeeMethods(_repository);
//            var user = new Employee("Admin", "User", "+123456789", "admin@example.com", "Системний Адміністратор", _workplace);
//            await _repository.AddAsync(_employee1);

//            // Act
//            await employeeMethods.PromoteToAdministratorAsync(user, _employee1);
//            var administrators = await _repository.GetByCriteriaAsync(e => e.Position == "Системний Адміністратор");

//            // Assert
//            Assert.AreEqual(1, administrators.Count());
//        }

//        [TestMethod]
//        public async Task PromoteEmployeeToAdministrator_ShouldUpdateRoleCorrectly()
//        {
//            // Arrange
//            var user = new Employee("Admin", "User", "+123456789", "admin@example.com", "Системний Адміністратор", _workplace);
//            await _repository.AddAsync(_employee1);

//            // Act
//            var employeeMethods = new EmployeeMethods(_repository);
//            await employeeMethods.PromoteToAdministratorAsync(user, _employee1);
//            var administrator = await _repository.GetByIdAsync(_employee1.ID);

//            // Assert
//            Assert.IsNotNull(administrator);
//            Assert.AreEqual(_employee1.FirstName, administrator.FirstName);
//            Assert.AreEqual("Системний Адміністратор", administrator.Position);
//        }


//        [TestMethod]
//        public async Task ManagerData_ShouldPersistCorrectly()
//        {
//            // Arrange
//            var workplace = new Warehouse(new Coordinates(50.45, 30.52, "Kyiv", "Ukraine"), 0, false);
//            var manager = new Employee("Manager", "NotUser", "+123456789", "manager@example.com", "Менеджер", _workplace);

//            manager.ManagedLocations = new List<BaseLocation>
//            {
//            workplace,
//            new Warehouse(new Coordinates(50, 30, "Kyiv", "Ukraine"), 0, true),
//            new Warehouse(new Coordinates(50.4, 30.5, "Kyiv", "Ukraine"), 50, false)

//            };

//            await _repository.AddAsync(manager);

//            // Act
//            var retrievedManager = await _repository.GetByIdAsync(manager.ID);

//            // Assert
//            Assert.IsNotNull(retrievedManager);
//            Assert.AreEqual(manager.FirstName, retrievedManager.FirstName);
//            Assert.AreEqual(manager.Position, retrievedManager.Position);
//            Assert.AreEqual(manager.ManagedLocations.Count, retrievedManager.ManagedLocations.Count);
//            Assert.AreEqual("Менеджер", retrievedManager.Position);
//        }

//        [TestMethod]
//        public async Task AdministratorData_ShouldPersistCorrectly()
//        {
//            // Arrange
//            var workplace = new Warehouse(new Coordinates(50.45, 30.52, "Kyiv", "Ukraine"), 0, false);
//            var administrator = new Employee("Admin", "User", "+123456789", "admin@example.com", "Системний Адміністратор", _workplace);

//            await _repository.AddAsync(administrator);

//            // Act
//            var retrievedAdministrator = await _repository.GetByIdAsync(administrator.ID);

//            // Assert
//            Assert.IsNotNull(retrievedAdministrator);
//            Assert.AreEqual(administrator.FirstName, retrievedAdministrator.FirstName);
//            Assert.AreEqual(administrator.Position, retrievedAdministrator.Position);
//            Assert.AreEqual("Системний Адміністратор", retrievedAdministrator.Position);
//        }


//    }
//}