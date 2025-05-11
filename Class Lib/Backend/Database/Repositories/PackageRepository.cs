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
using Class_Lib.Backend.Services;

namespace Class_Lib.Database.Repositories
{
    public class PackageRepository : Repository<Package>
    {
        public PackageRepository(AppDbContext context, User user) : base(context, user) { }

        // searching criteria:
        // by status
        public override async Task<IEnumerable<Package>> GetByCriteriaAsync(Expression<Func<Package, bool>> predicate)
        {
            //return await _context.Packages.Where(predicate).ToListAsync();
            //return await Query()
            //    .Include(p => p.DeclaredContent)
            //    .Where(predicate)
            //    .ExecuteAsync();

            if (_user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");

            if (!_user.HasPermission(AccessService.PermissionKey.ReadPackage))
                throw new UnauthorizedAccessException("Немає дозволу читати події.");

            var packages = await _context.Packages
                .Where(predicate)
                .ToListAsync();

            if (!packages.Any())
                return packages;

            var packageIds = packages.Select(e => e.ID).ToList();

            // Load related entities only if the user has permission

            if (_user.HasPermission(AccessService.PermissionKey.ReadContent))
            {
                await _context.Contents
                    .Where(c => packageIds.Contains(c.Package.ID))
                    .LoadAsync();
            }

            return packages;
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
