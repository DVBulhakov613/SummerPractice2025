using Class_Lib.Backend.Database;
using Class_Lib.Backend.Package_related.enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Database.Repositories
{
    public class EmployeeRepository : Repository<Employee>
    {
        public EmployeeRepository(AppDbContext context, Employee user) : base(context, user) { }

        public override async Task<IEnumerable<Employee>> GetByCriteriaAsync(Expression<Func<Employee, bool>> predicate)
        {
            return await Query()
                .Include(e => e.Role)
                .Include(e => e.Workplace)
                .Include(e => e.User)
                .Include(e => e.ManagedLocations)
                .Where(predicate)
                .ExecuteAsync();
        }

        // by ID
        public async Task<IEnumerable<Employee>> GetByIDAsync(uint id)
        {
            return await GetByCriteriaAsync(p => p.ID == id);
        }
        // by workplace ID
        public async Task<IEnumerable<Employee>> GetByWorkplaceIdAsync(uint workplaceId)
        {
            return await GetByCriteriaAsync(p => p.WorkplaceID == workplaceId);
        }

        // by first name
        public async Task<IEnumerable<Employee>> GetByFirstNameAsync(string firstName)
        {
            return await GetByCriteriaAsync(p => p.FirstName.ToLower() == firstName.ToLower());
        }

        // by last name
        public async Task<IEnumerable<Employee>> GetByLastNameAsync(string lastName)
        {
            return await GetByCriteriaAsync(p => p.Surname.ToLower() == lastName.ToLower());
        }

        // by full name
        public async Task<IEnumerable<Employee>> GetByFullNameAsync(string fullName)
        {
            return await GetByCriteriaAsync(p => p.FirstName.ToLower() + " " + p.Surname.ToLower() == fullName.ToLower());
        }

        // administrator count (for deletion restriction)
        public async Task<int> GetAdministratorCountAsync()
        {
            return await _context.Employees
                .Where(e => e.Role.Name == "Системний Адміністратор")
                .CountAsync();
        }
    }
}
