using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Database.Repositories
{
    public class LocationRepository : Repository<BaseLocation>
    {
        public LocationRepository(AppDbContext context, User user) : base(context, user) { }

        public override async Task<IEnumerable<BaseLocation>> GetByCriteriaAsync(Expression<Func<BaseLocation, bool>> predicate)
        {
            //return await _context.Locations.Where(predicate).ToListAsync();
            //return await Query()
            //    .Include(l => l.GeoData)
            //    .Include(l => l.Staff)
            //    .Where(predicate)
            //    .ExecuteAsync();

            if (_user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");

            if (!_user.HasPermission(AccessService.PermissionKey.ReadLocation))
                throw new UnauthorizedAccessException("Немає дозволу читати локації.");

            var locations = await _context.Locations
                .Where(predicate)
                .ToListAsync();

            if (!locations.Any())
                return locations;

            var locationIds = locations.Select(e => e.ID).ToList();

            // Load related entities only if the user has permission

            if (_user.HasPermission(AccessService.PermissionKey.ReadEmployee))
            {
                var employeesWithWorkplaces = await _context.Employees
                    .Where(e => e.Workplace != null)
                    .Select(e => (uint?)e.ID)
                    .ToListAsync();

                await _context.Employees
                    .Where(e => employeesWithWorkplaces.Contains(e.WorkplaceID))
                    .LoadAsync();
            }

            if (_user.HasPermission(AccessService.PermissionKey.ReadDelivery))
            {
                await _context.Deliveries
                    .Where(d => locationIds.Contains(d.SentFromID) || locationIds.Contains(d.SentToID))
                    .LoadAsync();
            }

            return locations;
        }
        public async Task<IEnumerable<BaseLocation>> GetLocationsByTypeAsync(string type)
        {
            return await _context.Locations
                .Where(l => l.GetType().Name == type)
                .ToListAsync();
        }
        public async Task<BaseLocation> GetLocationByIdAsync(int id)
        {
            return await _context.Locations.FindAsync(id);
        }
    }
}
