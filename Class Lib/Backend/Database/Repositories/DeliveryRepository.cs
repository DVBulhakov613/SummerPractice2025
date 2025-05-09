using Class_Lib.Backend.Package_related;
using Class_Lib.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Database.Repositories
{
    public class DeliveryRepository : Repository<Delivery>
    {
        public DeliveryRepository(AppDbContext context, Employee user) : base(context, user)
        {
        }

        public override async Task<IEnumerable<Delivery>> GetByCriteriaAsync(Expression<Func<Delivery, bool>> predicate)
        {
            return await Query()
                .Include(d => d.Package)
                .Include(d => d.Sender)
                .Include(d => d.Receiver)
                .Include(d => d.SentFrom)
                .Include(d => d.SentTo)
                .Include(d => d.Log)
                .Where(predicate)
                .ExecuteAsync();
        }
    }
}
