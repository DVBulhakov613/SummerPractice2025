using Class_Lib.Backend.Person_related;
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
        public LocationRepository(AppDbContext context, Employee user) : base(context, user) { }

        public override async Task<IEnumerable<BaseLocation>> GetByCriteriaAsync(Expression<Func<BaseLocation, bool>> predicate)
        {
            return await Query()
                .Include(l => l.GeoData)
                .Include(l => l.Staff)
                .Where(predicate)
                .ExecuteAsync();
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
