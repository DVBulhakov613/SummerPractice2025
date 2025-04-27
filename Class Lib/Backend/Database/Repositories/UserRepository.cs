using Class_Lib.Backend.Person_related;
using Microsoft.EntityFrameworkCore;

namespace Class_Lib.Backend.Database.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(AppDbContext context) : base(context) { }

        // for the query builder
        public QueryBuilder<User> Query()
        {
            return new QueryBuilder<User>(_context.Users);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Person) // include related Person data
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
