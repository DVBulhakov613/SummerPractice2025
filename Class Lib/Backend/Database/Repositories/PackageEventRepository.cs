using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Class_Lib.Backend.Database.Repositories
{
    public class PackageEventRepository : Repository<PackageEvent>
    {
        public PackageEventRepository(AppDbContext context, Employee user) : base(context, user) { }

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
