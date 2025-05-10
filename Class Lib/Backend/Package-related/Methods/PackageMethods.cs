using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Package_related.Methods
{
    public class PackageMethods
    {
        private readonly PackageRepository _packageRepository;

        public PackageMethods(PackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        // Create
        public async Task AddAsync(Employee user, Package package)
        {
            if (!user.HasPermission(AccessService.PermissionKey.CreatePackage))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення посилки.");
            }

            await _packageRepository.AddAsync(package);
        }

        // Read
        public async Task<IEnumerable<Package>> GetByCriteriaAsync(Employee user, Expression<Func<Package, bool>> filter)
        {
            if (!user.HasPermission(AccessService.PermissionKey.ReadPackage))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду посилок.");
            }

            return await _packageRepository.Query()
                .Where(filter)
                .ExecuteAsync();
        }

        // Update
        public async Task UpdateAsync(Employee user, Package package)
        {
            if (!user.HasPermission(AccessService.PermissionKey.UpdatePackage))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати посилки.");
            }

            await _packageRepository.UpdateAsync(package);
        }

        // Delete
        public async Task DeleteAsync(Employee user, Package package)
        {
            if (!user.HasPermission(AccessService.PermissionKey.DeletePackage))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти посилки.");
            }

            await _packageRepository.DeleteAsync(package);
        }
    }
}
