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

        // generic query method
        public async Task<IEnumerable<Employee>> GetEmployeesByCriteria(Expression<Func<Employee, bool>> predicate)
        {
            return await _context.Employees
                .Where(predicate)
                .ToListAsync();
        }

        // searching criteria:

        // 1. by ID
        public async Task<IEnumerable<Employee>> GetEmployeesByIDAsync(uint id)
        {
            return await GetEmployeesByCriteria(p => p.ID == id);
        }
        // 2. by workplace ID
        public async Task<IEnumerable<Employee>> GetEmployeesByWorkplaceIdAsync(uint workplaceId)
        {
            return await GetEmployeesByCriteria(p => p.WorkplaceID == workplaceId);
        }

        // 3. by first name
        public async Task<IEnumerable<Employee>> GetEmployeesByFirstNameAsync(string firstName)
        {
            return await GetEmployeesByCriteria(p => p.Name.ToLower() == firstName.ToLower());
        }

        // 4. by last name
        public async Task<IEnumerable<Employee>> GetEmployeesByLastNameAsync(string lastName)
        {
            return await GetEmployeesByCriteria(p => p.Surname.ToLower() == lastName.ToLower());
        }

        // 5. by full name
        public async Task<IEnumerable<Employee>> GetEmployeesByFullNameAsync(string fullName)
        {
            return await GetEmployeesByCriteria(p => p.Name.ToLower() + " " + p.Surname.ToLower() == fullName.ToLower());
        }

        // get all
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .ToListAsync();
        }
    }
}
