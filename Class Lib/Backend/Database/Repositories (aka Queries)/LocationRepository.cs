using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Database.Repositories
{
    internal class LocationRepository : Repository<BaseLocation>
    {
        AppDbContext _context;
        public LocationRepository(AppDbContext context) : base(context) { }
        // Add any specific methods for LocationRepository here
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
