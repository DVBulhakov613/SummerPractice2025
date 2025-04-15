using Microsoft.EntityFrameworkCore;
using Class_Lib.Backend.Package_related.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Class_Lib.Backend.Person_related;

namespace Class_Lib.Database.Repositories
{
    public class PackageRepository : Repository<Package>
    {
        public PackageRepository(AppDbContext context) : base(context) { }

        // generic query method
        public async Task<IEnumerable<Package>> GetPackagesByCriteriaAsync(Expression<Func<Package, bool>> predicate)
        {
            return await _context.Packages
                .Where(predicate)
                .ToListAsync();
        }

        // searching criteria:
        // 1. By status
        public async Task<IEnumerable<Package>> GetPackagesByStatusAsync(PackageStatus status)
        {
            return await GetPackagesByCriteriaAsync(p => p.PackageStatus == status);
        }

        // 2. By starting point
        public async Task<IEnumerable<Package>> GetPackagesByStartingPoint(PostalOffice startingPoint)
        {
            return await GetPackagesByCriteriaAsync(p => p.SentFrom == startingPoint);
        }

        // 3. By destination
        public async Task<IEnumerable<Package>> GetPackagesByDestination(PostalOffice destination)
        {
            return await GetPackagesByCriteriaAsync(p => p.SentTo == destination);
        }

        // 4. By sender
        public async Task<IEnumerable<Package>> GetPackagesBySender(Client sender)
        {
            return await GetPackagesByCriteriaAsync(p => p.Sender == sender);
        }

        // 5. By receiver
        public async Task<IEnumerable<Package>> GetPackagesByReceiver(Client receiver)
        {
            return await GetPackagesByCriteriaAsync(p => p.Receiver == receiver);
        }

        // 6. By content (?)
        // to-do

        // 7. By package type
        public async Task<IEnumerable<Package>> GetPackagesByPackageType(PackageType packageType)
        {
            return await GetPackagesByCriteriaAsync(p => p.Type == packageType);
        }

        // 8. By package ID (already in Repository)

        // 9. By package weight
        public async Task<IEnumerable<Package>> GetPackagesByWeight(uint weight)
        {
            return await GetPackagesByCriteriaAsync(p => p.Weight == weight);
        }

        public async Task<IEnumerable<Package>> GetPackagesByWeightRangeAsync(uint minWeight, uint maxWeight)
        {
            return await GetPackagesByCriteriaAsync(p => p.Weight >= minWeight && p.Weight <= maxWeight);
        }

        // 10. By package volume
        public async Task<IEnumerable<Package>> GetPackagesByVolume(uint volume)
        {
            return await GetPackagesByCriteriaAsync(p => p.Volume == volume);
        }

        public async Task<IEnumerable<Package>> GetPackagesByVolumeRangeAsync(uint minVolume, uint maxVolume)
        {
            return await GetPackagesByCriteriaAsync(p => p.Volume >= minVolume && p.Volume <= maxVolume);
        }

        // 11. By package dimensions (length, width, height) (nvm not possible as they are private)

        // 12. By package creation date
        public async Task<IEnumerable<Package>> GetPackagesByCreationDate(DateTime time)
        {
            return await GetPackagesByCriteriaAsync(p => p.CreatedAt == time);
        }

        public async Task<IEnumerable<Package>> GetPackagesByCreationDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await GetPackagesByCriteriaAsync(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate);
        }

        // 13. By package delivery date (?)

    }
}
