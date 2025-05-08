using Class_Lib.Backend.Database;
using Class_Lib.Backend.Person_related;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Database.Repositories
{
    public class ClientRepository : Repository<Client>
    {
        public ClientRepository(AppDbContext context, Employee user) : base(context, user) { }

        public override async Task<IEnumerable<Client>> GetByCriteriaAsync(Expression<Func<Client, bool>> predicate)
        {
            return await Query()
                .Include(c => c.PackagesSent)
                .Include(c => c.PackagesReceived)
                .Where(predicate)
                .ExecuteAsync();
        }

        // by ID
        public async Task<IEnumerable<Client>> GetClientsByIDAsync(uint id)
        {
            return await GetByCriteriaAsync(p => p.ID == id);
        }

        // by first name
        public async Task<IEnumerable<Client>> GetClientsByFirstNameAsync(string firstName)
        {
            return await GetByCriteriaAsync(p => p.FirstName.ToLower() == firstName.ToLower());
        }

        // by last name
        public async Task<IEnumerable<Client>> GetClientsByLastNameAsync(string lastName)
        {
            return await GetByCriteriaAsync(p => p.Surname.ToLower() == lastName.ToLower());
        }

        // by full name
        public async Task<IEnumerable<Client>> GetClientsByFullNameAsync(string fullName)
        {
            return await GetByCriteriaAsync(p => p.FirstName.ToLower() + " " + p.Surname.ToLower() == fullName.ToLower());
        }
    }
}
