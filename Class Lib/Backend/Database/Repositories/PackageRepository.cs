using Microsoft.EntityFrameworkCore;
using Class_Lib.Backend.Package_related.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Database;

namespace Class_Lib.Database.Repositories
{
    public class PackageRepository : Repository<Package>
    {
        public PackageRepository(AppDbContext context) : base(context) { }

        // for the query builder
        public QueryBuilder<Package> Query()
        {
            return new QueryBuilder<Package>(_context.Packages);
        }

        // generic query method
        public async Task<IEnumerable<Package>> GetPackagesByCriteriaAsync(Expression<Func<Package, bool>> predicate)
        {
            return await _context.Packages
                .Where(predicate)
                .ToListAsync();
        }

        // searching criteria:

        // by ID
        public async Task<IEnumerable<Package>> GetPackageByIDAsync(uint id)
        {
            return await GetPackagesByCriteriaAsync(p => p.ID == id);
        }
        // by status
        public async Task<IEnumerable<Package>> GetPackagesByStatusAsync(PackageStatus status)
        {
            return await GetPackagesByCriteriaAsync(p => p.PackageStatus == status);
        }

        // by starting point
        public async Task<IEnumerable<Package>> GetPackagesByStartingPoint(PostalOffice startingPoint)
        {
            return await GetPackagesByCriteriaAsync(p => p.SentFrom == startingPoint);
        }

        // by destination
        public async Task<IEnumerable<Package>> GetPackagesByDestination(PostalOffice destination)
        {
            return await GetPackagesByCriteriaAsync(p => p.SentTo == destination);
        }

        // by sender
        public async Task<IEnumerable<Package>> GetPackagesBySender(Client sender)
        {
            return await GetPackagesByCriteriaAsync(p => p.Sender == sender);
        }

        // by receiver
        public async Task<IEnumerable<Package>> GetPackagesByReceiver(Client receiver)
        {
            return await GetPackagesByCriteriaAsync(p => p.Receiver == receiver);
        }

        // by content (?)
        // to-do

        // by package type
        public async Task<IEnumerable<Package>> GetPackagesByPackageType(PackageType packageType)
        {
            return await GetPackagesByCriteriaAsync(p => p.Type == packageType);
        }

        // by package ID (already in Repository)

        // by package weight
        public async Task<IEnumerable<Package>> GetPackagesByWeightAsync(uint weight)
        {
            return await GetPackagesByCriteriaAsync(p => p.Weight == weight);
        }

        public async Task<IEnumerable<Package>> GetPackagesByWeightRangeAsync(uint minWeight, uint maxWeight)
        {
            return await GetPackagesByCriteriaAsync(p => p.Weight >= minWeight && p.Weight <= maxWeight);
        }

        // by package volume
        public async Task<IEnumerable<Package>> GetPackagesByVolumeAsync(uint volume)
        {
            return await GetPackagesByCriteriaAsync(p => p.Volume == volume);
        }

        public async Task<IEnumerable<Package>> GetPackagesByVolumeRangeAsync(uint minVolume, uint maxVolume)
        {
            return await GetPackagesByCriteriaAsync(p => p.Volume >= minVolume && p.Volume <= maxVolume);
        }

        // by package dimensions (length, width, height) (nvm not possible as they are private)

        // by package creation date
        public async Task<IEnumerable<Package>> GetPackagesByCreationDateAsync(DateTime time)
        {
            return await GetPackagesByCriteriaAsync(p => p.CreatedAt == time);
        }

        public async Task<IEnumerable<Package>> GetPackagesByCreationDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await GetPackagesByCriteriaAsync(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate);
        }

        // by package delivery date (?)

    }
}
