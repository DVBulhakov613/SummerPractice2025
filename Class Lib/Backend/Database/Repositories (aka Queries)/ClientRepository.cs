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
        public ClientRepository(AppDbContext context) : base(context) { }

        // generic query method
        public async Task<IEnumerable<Client>> GetClientsByCriteria(Expression<Func<Client, bool>> predicate)
        {
            return await _context.Clients
                .Where(predicate)
                .ToListAsync();
        }

        // searching criteria:

        // 1. by ID
        public async Task<IEnumerable<Client>> GetClientsByIDAsync(uint id)
        {
            return await GetClientsByCriteria(p => p.ID == id);
        }

        // 2. by first name
        public async Task<IEnumerable<Client>> GetClientsByFirstNameAsync(string firstName)
        {
            return await GetClientsByCriteria(p => p.Name.ToLower() == firstName.ToLower());
        }

        // 3. by last name
        public async Task<IEnumerable<Client>> GetClientsByLastNameAsync(string lastName)
        {
            return await GetClientsByCriteria(p => p.Surname.ToLower() == lastName.ToLower());
        }

        // 4. by full name
        public async Task<IEnumerable<Client>> GetClientsByFullNameAsync(string fullName)
        {
            return await GetClientsByCriteria(p => p.Name.ToLower() + " " + p.Surname.ToLower() == fullName.ToLower());
        }

        // get all
        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _context.Clients
                .ToListAsync();
        }
    }
}
