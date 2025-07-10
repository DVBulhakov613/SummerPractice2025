using Class_Lib.Backend.Services;
using Microsoft.EntityFrameworkCore;

namespace Class_Lib.Backend.Database.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        public Task<Role> GetRoleByIdAsync(uint roleId);

        public Task<Role> GetRoleByNameAsync(string roleName);
    }
}