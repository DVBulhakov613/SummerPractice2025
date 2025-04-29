using Class_Lib.Backend.Person_related;
using Microsoft.EntityFrameworkCore;

namespace Class_Lib.Backend.Database.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(AppDbContext context) : base(context) { }

        // for the query builder
        public QueryBuilderService<User> Query()
        {
            return new QueryBuilderService<User>(_context.Users);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Employee) // include related Person data
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
