using Class_Lib;
using Class_Lib.Backend.Database;
using Class_Lib.Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OOP_CourseProject_TestProject.Class_tests
{
    public class TestTemplate
    {
        protected AppDbContext _context;
        protected RoleService _roleService;
        protected IServiceProvider _provider;
        protected User _adminUser = new("adminUser", PasswordHelper.HashPassword("defaultpassword"), new Role { Name = "Admin", ID = 1 }, new("Admin", "Admin", "+000000000", "admin@domain.com", null));
        protected User _unauth = new("unauth", PasswordHelper.HashPassword("defaultpassword"), new Role { Name = "unauth", ID = 999999 }, new("unauth", "unauth", "+000000000", "unauth@unauth.uat", null));

        protected virtual void Setup()
        {
            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));

            services.AddBackendServices(_adminUser);

            _provider = services.BuildServiceProvider();
            _context = _provider.GetRequiredService<AppDbContext>();
            _roleService = _provider.GetRequiredService<RoleService>();
            _adminUser.CachedPermissions = Enum.GetValues(typeof(AccessService.PermissionKey))
                             .Cast<int>()
                             .ToList();
        }

        protected virtual void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }

}
