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
using Class_Lib.Backend.Package_related;

namespace Class_Lib.Database.Repositories
{
    public class PackageRepository : Repository<Package>
    {
        public PackageRepository(AppDbContext context, Employee user) : base(context, user) { }

        // searching criteria:
        // by status
        public override async Task<IEnumerable<Package>> GetByCriteriaAsync(Expression<Func<Package, bool>> predicate)
        {
            return await Query()
                .Include(p => p.DeclaredContent)
                .Where(predicate)
                .ExecuteAsync();
        }
        public async Task<IEnumerable<Package>> GetByStatusAsync(PackageStatus status)
        {
            return await GetByCriteriaAsync(p => p.PackageStatus == status);
        }

        // by content (?)
        // to-do

        // by package type
        public async Task<IEnumerable<Package>> GetByTypeAsync(PackageType packageType)
        {
            return await GetByCriteriaAsync(p => p.Type == packageType);
        }

        // by package ID (already in Repository)

        // by package weight
        public async Task<IEnumerable<Package>> GetByWeightAsync(uint weight)
        {
            return await GetByCriteriaAsync(p => p.Weight == weight);
        }

        public async Task<IEnumerable<Package>> GetByWeightRangeAsync(uint minWeight, uint maxWeight)
        {
            return await GetByCriteriaAsync(p => p.Weight >= minWeight && p.Weight <= maxWeight);
        }

        // by package volume
        public async Task<IEnumerable<Package>> GetByVolumeAsync(uint volume)
        {
            return await GetByCriteriaAsync(p => p.Volume == volume);
        }

        public async Task<IEnumerable<Package>> GetByVolumeRangeAsync(uint minVolume, uint maxVolume)
        {
            return await GetByCriteriaAsync(p => p.Volume >= minVolume && p.Volume <= maxVolume);
        }

        // by package dimensions (length, width, height) (nvm not possible as they are private)

        // by package creation date
        public async Task<IEnumerable<Package>> GetByCreationDateAsync(DateTime time)
        {
            return await GetByCriteriaAsync(p => p.CreatedAt.Date == time.Date);
        }

        public async Task<IEnumerable<Package>> GetByCreationDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await GetByCriteriaAsync(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate);
        }

        // by package delivery date (?)

    }
}
