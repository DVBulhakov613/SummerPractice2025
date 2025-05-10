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

        public async Task<IEnumerable<PackageEvent>> GetAllAsync(Employee user)
        {
            if(!user.HasPermission(Services.AccessService.PermissionKey.ReadEvent))
                throw new UnauthorizedAccessException("Немає дозволу до журналу подій.");

            return await _packageEventRepository.Query()
                .Include(e => e.Package)
                .Include(e => e.Location)
                .ExecuteAsync();
        }

        public async Task<IEnumerable<PackageEvent>> GetAllFromDeliveryAsync(Employee user, Delivery delivery)
        {
            if(delivery == null) { throw new ArgumentNullException(""); };
            if (!user.HasPermission(Services.AccessService.PermissionKey.ReadEvent))
                throw new UnauthorizedAccessException("Немає дозволу до журналу подій.");
            return await _packageEventRepository.GetByCriteriaAsync(e => e.Package.Delivery.ID == delivery.ID);
        }

        public async Task<IEnumerable<PackageEvent>> GetAllFromPackageAsync(Employee user, Package package)
        {
            if(package == null) { throw new ArgumentNullException(""); };
            if (!user.HasPermission(Services.AccessService.PermissionKey.ReadEvent))
                throw new UnauthorizedAccessException("Немає дозволу до журналу подій.");
            return await _packageEventRepository.GetByCriteriaAsync(e => e.Package.ID == package.ID);
        }
    }
}
