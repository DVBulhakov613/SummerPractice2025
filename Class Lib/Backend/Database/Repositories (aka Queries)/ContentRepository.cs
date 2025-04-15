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
    public class ContentRepository : Repository<Content>
    {
        public ContentRepository(AppDbContext context) : base(context)
        {
        }
        // are there any specific context methods though
        // yes there are

        public async Task<IEnumerable<Content>> GetContentsByCriteriaAsync(Expression<Func<Content, bool>> predicate)
        {
            return await _context.Contents
                .Where(predicate)
                .ToListAsync();
        }

        // 1. Get all content by name
        public async Task<IEnumerable<Content>> GetAllContentByNameAsync(string type)
        {
            return await GetContentsByCriteriaAsync(c => c.GetType().Name == type);
        }

        // 2. Get all content by type
        public async Task<IEnumerable<Content>> GetAllContentByTypeAsync(ContentType type)
        {
            return await GetContentsByCriteriaAsync(c => c.Type == type);
        }

        // 3. Get all content by package
        public async Task<IEnumerable<Content>> GetAllContentByPackageAsync(Package package)
        {
            return await GetContentsByCriteriaAsync(c => c.Package == package);
        }

        // 4. Get all content by sender
        public async Task<IEnumerable<Content>> GetAllContentBySenderAsync(Client sender)
        {
            return await GetContentsByCriteriaAsync(c => c.Package.Sender == sender);
        }

        // 5. Get all content by receiver
        public async Task<IEnumerable<Content>> GetAllContentByReceiverAsync(Client receiver)
        {
            return await GetContentsByCriteriaAsync(c => c.Package.Receiver == receiver);
        }

        // 6. Get all content
        public async Task<IEnumerable<Content>> GetAllContentsAsync()
        {
            return await _context.Contents.ToListAsync();
        }
    }
}
