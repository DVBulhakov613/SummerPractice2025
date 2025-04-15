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

        public async Task AddPackageSentAsync(Person user, Package package, Client client)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "CreatePackage"))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення посилки.");
            }

            client.PackagesSent.Add(package);

            await _packageRepository.AddAsync(package);
        }

        public async Task AddPackageReceivedAsync(Person user, Package package, Client client)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "CreatePackage"))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення посилки.");
            }

            client.PackagesReceived.Add(package);

            await _packageRepository.AddAsync(package);
        }

        public async Task DeletePackageSentAsync(Person user, Package package)
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

            package.Sender.PackagesSent.Remove(package);
            package.Receiver.PackagesSent.Remove(package);
            await _packageRepository.DeleteAsync(package);

        }

        public async Task DeletePackageReceivedAsync(Person user, Package package)
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

            package.Sender.PackagesReceived.Remove(package);
            package.Receiver.PackagesReceived.Remove(package);
            await _packageRepository.DeleteAsync(package);

        }
    }
}
