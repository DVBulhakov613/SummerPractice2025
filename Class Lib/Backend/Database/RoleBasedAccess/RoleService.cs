using Class_Lib.Backend.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Class_Lib.Backend.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CachePermissionsAsync(User employee)
        {
            if (employee.Role == null || employee.RoleID == 0)
                throw new InvalidOperationException("Працівник не має ролі.");

            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleID == employee.RoleID)
                .Select(rp => rp.PermissionID)
                .ToListAsync();

            employee.CachedPermissions = permissions.Select(p => (int)p).ToList();
        }
    }
}
