using Class_Lib.Backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Class_Lib.Backend.Database.Repositories
{
    public class PermissionRepository : Repository<Permission>
    {
        public PermissionRepository(AppDbContext context) : base(context)
        { }

        // very simple permission pull for now
        public override async Task<IEnumerable<Permission>> GetByCriteriaAsync(Expression<Func<Permission, bool>> predicate)
        {
            if (_user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");

            if (!_user.HasPermission(AccessService.PermissionKey.ReadPermission))
                throw new UnauthorizedAccessException("Немає дозволу читати дозволи ролей.");

            var permissions = await _context.Permissions
                .Where(predicate)
                .ToListAsync();

            return permissions;
        }

        public Task<Permission> GetRoleByIdAsync(uint roleId)
        {
            return _context.Permissions.FirstOrDefaultAsync(r => r.ID == roleId);
        }

        public Task<Permission> GetRoleByNameAsync(string roleName)
        {
            return _context.Permissions.FirstOrDefaultAsync(r => r.Name == roleName);
        }
    }
}
