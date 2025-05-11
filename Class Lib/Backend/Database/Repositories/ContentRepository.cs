using Class_Lib.Backend.Package_related;
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
    public class ContentRepository : Repository<Content>
    {
        public ContentRepository(AppDbContext context, User user) : base(context, user) { }

        public override async Task<IEnumerable<Content>> GetByCriteriaAsync(Expression<Func<Content, bool>> predicate)
        {
            if (_user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");

            // Always fetch contents first
            if (!_user.HasPermission(AccessService.PermissionKey.ReadContent))
                throw new UnauthorizedAccessException("Немає дозволу читати зміст посилок.");

            var contents = await _context.Contents
                .Where(predicate)
                .ToListAsync();

            return contents;
        }

        public async Task<IEnumerable<Content>> GetAllContentByNameAsync(string type)
        {
            return await GetByCriteriaAsync(c => c.GetType().Name == type);
        }

        // get all content by type
        public async Task<IEnumerable<Content>> GetAllContentByTypeAsync(ContentType type)
        {
            return await GetByCriteriaAsync(c => c.Type == type);
        }

        // get all content by package
        public async Task<IEnumerable<Content>> GetAllContentByPackageAsync(Package package)
        {
            return await GetByCriteriaAsync(c => c.Package == package);
        }
    }
}
