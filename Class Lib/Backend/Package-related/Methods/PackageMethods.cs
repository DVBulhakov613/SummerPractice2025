using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Package_related
{
    public class PackageMethods
    {
        private readonly PackageRepository _packageRepository;

        public PackageMethods(PackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public async Task<IEnumerable<Package>> GetAllPackagesAsync()
        {
            return await _packageRepository.GetAllAsync();
        }

        public async Task AddPackageAsync(Person user, Package package, Client client)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "CreatePackage"))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення посилки.");
            }

            client.Packages.Add(package);

            await _packageRepository.AddAsync(package);
        }

        public async Task DeletePackageAsync(Person user, Package package)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "DeletePackage"))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти посилки.");
            }

            if (package.PackageStatus != PackageStatus.DELIVERED &&
                package.PackageStatus != PackageStatus.CANCELED &&
                package.PackageStatus != PackageStatus.RETURNED)
            {
                throw new InvalidOperationException("Не можна видаляти посилку яку не: доставили, відмінили, або повернули.");
            }

            package.Sender.Packages.Remove(package);
            package.Receiver.Packages.Remove(package);
            await _packageRepository.DeleteAsync(package);

        }
    }
}
