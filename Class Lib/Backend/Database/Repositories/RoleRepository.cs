using Class_Lib.Backend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Database.Repositories
{
    public class RoleRepository : Repository<Role>
    {
        public RoleRepository(AppDbContext context) : base(context)
        { }

        public async Task<IEnumerable<Role>> GetByCriteriaAsync(User user, Expression<Func<Role, bool>> predicate)
        {
            var roles = await _context.Roles
                .Where(predicate)
                .ToListAsync();

            if (!roles.Any())
                return roles;

            var roleIds = roles.Select(r => r.ID).ToList();

            if (user != null && user.HasPermission(AccessService.PermissionKey.ReadRolePermissions))
            {
                await _context.RolePermissions
                    .Where(rp => roleIds.Contains(rp.RoleID))
                    .LoadAsync();
            }

            if (user != null && user.HasPermission(AccessService.PermissionKey.ReadUser))
            {
                await _context.Users
                    .Where(u => u.RoleID != null && roleIds.Contains((uint)u.RoleID))
                    .LoadAsync();
            }

            return roles;
        }

        public Task<Role> GetRoleByIdAsync(uint roleId)
        {
            return _context.Roles.FirstOrDefaultAsync(r => r.ID == roleId);
        }

        public Task<Role> GetRoleByNameAsync(string roleName)
        {
            return _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }
    }
}
