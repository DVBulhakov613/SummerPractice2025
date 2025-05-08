using Class_Lib.Backend.Package_related;
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
        public ContentRepository(AppDbContext context, Employee user) : base(context, user) { }

        // get all content by name
        public override async Task<IEnumerable<Content>> GetByCriteriaAsync(Expression<Func<Content, bool>> predicate)
        {
            return await Query()
                .Include(c => c.Package)
                .Include(c => c.Package.Sender)
                .Include(c => c.Package.Receiver)
                .Include(c => c.Package.DeclaredContent)
                .Where(predicate)
                .ExecuteAsync();
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

        // get all content by sender
        public async Task<IEnumerable<Content>> GetAllContentBySenderAsync(Client sender)
        {
            return await GetByCriteriaAsync(c => c.Package.Sender == sender);
        }

        // get all content by receiver
        public async Task<IEnumerable<Content>> GetAllContentByReceiverAsync(Client receiver)
        {
            return await GetByCriteriaAsync(c => c.Package.Receiver == receiver);
        }
    }
}
