using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Database.Methods
{
    public class PermissionMethods
    {
        private readonly PermissionRepository _PermissionRepository;

        public PermissionMethods(PermissionRepository PermissionRepository)
        {
            _PermissionRepository = PermissionRepository;
        }

        // Create
        public async Task AddAsync(User user, Permission package)
        {
            if (!user.HasPermission(AccessService.PermissionKey.CreatePermission))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення дозволу.");
            }

            await _PermissionRepository.AddAsync(package);
        }

        // Read
        public async Task<IEnumerable<Permission>> GetByCriteriaAsync(User user, Expression<Func<Permission, bool>> filter)
        {
            if (!user.HasPermission(AccessService.PermissionKey.ReadPermission))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду дозволу.");
            }

            return await _PermissionRepository.GetByCriteriaAsync(filter);
        }

        // Update
        public async Task UpdateAsync(User user, Permission package)
        {
            if (!user.HasPermission(AccessService.PermissionKey.UpdatePermission))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати дозвіл.");
            }

            await _PermissionRepository.UpdateAsync(package);
        }

        // Delete
        public async Task DeleteAsync(User user, Permission package)
        {
            if (!user.HasPermission(AccessService.PermissionKey.DeletePermission))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти дозвіл.");
            }

            await _PermissionRepository.DeleteAsync(package);
        }

        // Get all permissions
        public async Task<IEnumerable<Permission>> GetAllAsync(User user)
        {
            if (!user.HasPermission(AccessService.PermissionKey.ReadPermission))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду дозволів.");
            }
            return await _PermissionRepository.GetAllAsync();
        }
    }
}
