using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Services;
using System.Linq.Expressions;

namespace Class_Lib.Backend.Database.Methods
{
    public class RoleMethods
    {
        private readonly RoleRepository _roleRepository;

        public RoleMethods(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        // Create
        public async Task AddAsync(User user, Role role)
        {
            if (!user.HasPermission(AccessService.PermissionKey.CreateRole))
                throw new UnauthorizedAccessException("Немає доступу до створення ролі.");

            await _roleRepository.AddAsync(role);
        }

        // Read
        public async Task<IEnumerable<Role>> GetByCriteriaAsync(User user, Expression<Func<Role, bool>> filter)
        {
            if(user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");

            if (!user.HasPermission(AccessService.PermissionKey.ReadRole))
                throw new UnauthorizedAccessException("Немає доступу до перегляду ролей.");

            return await _roleRepository.GetByCriteriaAsync(user, filter);
        }

        // Update
        public async Task UpdateAsync(User user, Role role)
        {
            if (!user.HasPermission(AccessService.PermissionKey.UpdateRole))
                throw new UnauthorizedAccessException("Немає дозволу змінювати ролі.");

            await _roleRepository.UpdateAsync(role);
        }

        // Delete
        public async Task DeleteAsync(User user, Role role)
        {
            if (!user.HasPermission(AccessService.PermissionKey.DeleteRole))
                throw new UnauthorizedAccessException("Немає дозволу видаляти ролі.");

            await _roleRepository.DeleteAsync(role);
        }
    }
}
