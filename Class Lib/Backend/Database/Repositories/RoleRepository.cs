using Class_Lib.Backend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Database.Repositories
{
    public class RoleRepository : Repository<Role>
    {
        public RoleRepository(AppDbContext context) : base(context)
        { }

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
