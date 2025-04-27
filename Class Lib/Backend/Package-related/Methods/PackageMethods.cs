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
        public async Task AddPackageAsync(Employee user, Package package)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "CreatePackage"))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення посилки.");
            }

            await _packageRepository.AddAsync(package);
        }

        // Read
        public async Task<IEnumerable<Package>> GetPackagesByCustomCriteriaAsync(Employee user, Expression<Func<Package, bool>> filter)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "ReadPackage"))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду посилок.");
            }

            return await _packageRepository.Query()
                .Where(filter)
                .ExecuteAsync();
        }

        // Update
        public async Task UpdatePackageAsync(Employee user, Package package)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "UpdatePackage"))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати посилки.");
            }

            await _packageRepository.UpdateAsync(package);
        }

        // Delete
        public async Task DeletePackageAsync(Employee user, Package package)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "DeletePackage"))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти посилки.");
            }

            await _packageRepository.DeleteAsync(package);
        }
    }
}
