using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Package_related;

namespace Class_Lib.Backend.Database.Methods
{
    public class PackageEventMethods
    {
        private readonly PackageEventRepository _packageEventRepository;

        public PackageEventMethods(PackageEventRepository packageEventRepository)
        {
            _packageEventRepository = packageEventRepository;
        }

        public async Task<IEnumerable<PackageEvent>> GetAllAsync(User user)
        {
            if(!user.HasPermission(Services.AccessService.PermissionKey.ReadEvent))
                throw new UnauthorizedAccessException("Немає дозволу до журналу подій.");

            return await _packageEventRepository.GetByCriteriaAsync(e => true);
        }

        public async Task<IEnumerable<PackageEvent>> GetAllFromDeliveryAsync(User user, Delivery delivery)
        {
            if(delivery == null) { throw new ArgumentNullException(""); };
            if (!user.HasPermission(Services.AccessService.PermissionKey.ReadEvent))
                throw new UnauthorizedAccessException("Немає дозволу до журналу подій.");
            return await _packageEventRepository.GetByCriteriaAsync(e => e.Package.Delivery.ID == delivery.ID);
        }

        public async Task<IEnumerable<PackageEvent>> GetAllFromPackageAsync(User user, Package package)
        {
            if(package == null) { throw new ArgumentNullException(""); };
            if (!user.HasPermission(Services.AccessService.PermissionKey.ReadEvent))
                throw new UnauthorizedAccessException("Немає дозволу до журналу подій.");
            return await _packageEventRepository.GetByCriteriaAsync(e => e.Package.ID == package.ID);
        }
    }
}
