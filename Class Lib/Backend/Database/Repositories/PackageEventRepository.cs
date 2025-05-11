using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Class_Lib.Backend.Database.Repositories
{
    public class PackageEventRepository : Repository<PackageEvent>
    {
        public PackageEventRepository(AppDbContext context, User user) : base(context, user) { }

        public override async Task<IEnumerable<PackageEvent>> GetByCriteriaAsync(Expression<Func<PackageEvent, bool>> predicate)
        {
            //return await _context.PackageEvents.Where(predicate).ToListAsync();
            //return await Query()
            //    .Include(pe => pe.Package)
            //    .Include(pe => pe.Location)
            //    .Where(predicate)
            //    .ExecuteAsync();

            if (_user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");

            if (!_user.HasPermission(AccessService.PermissionKey.ReadEvent))
                throw new UnauthorizedAccessException("Немає дозволу читати події.");

            var events = await _context.PackageEvents
                .Where(predicate)
                .ToListAsync();

            if (!events.Any())
                return events;

            var eventIds = events.Select(e => e.PackageID).ToList();

            // Load related entities only if the user has permission

            if(_user.HasPermission(AccessService.PermissionKey.ReadPackage))
            {
                await _context.Packages
                    .Where(p => eventIds.Contains(p.ID))
                    .LoadAsync();
            }

            if (_user.HasPermission(AccessService.PermissionKey.ReadLocation))
            {
                var locationIds = events.Select(e => e.LocationID).Distinct().ToList();
                await _context.Locations
                    .Where(l => locationIds.Contains(l.ID))
                    .LoadAsync();
            }

            return events;
        }

        // generic query method
        public async Task<IEnumerable<PackageEvent>> GetPackageEventsByCriteriaAsync(Expression<Func<PackageEvent, bool>> predicate)
        {
            return await _context.PackageEvents
                .Where(predicate)
                .ToListAsync();
        }

       // will leave it be with the generic method for now
    }
}
