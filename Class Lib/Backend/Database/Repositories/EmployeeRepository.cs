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
        public EmployeeRepository(AppDbContext context) : base(context) { }

        // for the query builder
        public QueryBuilder<Employee> Query()
        {
            return new QueryBuilder<Employee>(_context.Employees);
        }

        // generic query method
        public async Task<IEnumerable<Employee>> GetEmployeesByCriteria(Expression<Func<Employee, bool>> predicate)
        {
            return await _context.Employees
                .Where(predicate)
                .ToListAsync();
        }

        // searching criteria:

        // by ID
        public async Task<IEnumerable<Employee>> GetEmployeesByIDAsync(uint id)
        {
            return await GetEmployeesByCriteria(p => p.ID == id);
        }
        // by workplace ID
        public async Task<IEnumerable<Employee>> GetEmployeesByWorkplaceIdAsync(uint workplaceId)
        {
            return await GetEmployeesByCriteria(p => p.WorkplaceID == workplaceId);
        }

        // by first name
        public async Task<IEnumerable<Employee>> GetEmployeesByFirstNameAsync(string firstName)
        {
            return await GetEmployeesByCriteria(p => p.FirstName.ToLower() == firstName.ToLower());
        }

        // by last name
        public async Task<IEnumerable<Employee>> GetEmployeesByLastNameAsync(string lastName)
        {
            return await GetEmployeesByCriteria(p => p.Surname.ToLower() == lastName.ToLower());
        }

        // by full name
        public async Task<IEnumerable<Employee>> GetEmployeesByFullNameAsync(string fullName)
        {
            return await GetEmployeesByCriteria(p => p.FirstName.ToLower() + " " + p.Surname.ToLower() == fullName.ToLower());
        }

        // administrator count (for deletion restriction)
        public async Task<int> GetAdministratorCountAsync()
        {
            return await _context.Employees.OfType<Administrator>().CountAsync();
        }
    }
}
