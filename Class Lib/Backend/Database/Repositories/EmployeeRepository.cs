using Class_Lib.Backend.Database;
using Class_Lib.Backend.Database.Interfaces;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Database.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context, User user) : base(context, user) { }

        //public override async Task<IEnumerable<Employee>> GetByCriteriaAsync(Expression<Func<Employee, bool>> predicate)
        //{
        //    return await _context.Employees.Where(predicate)
        //        .Include(e => e.Role)
        //            .ThenInclude(r => r.RolePermissions)
        //                .ThenInclude(rp => rp.Permission)
        //        .Include(e => e.Workplace)
        //        .Include(e => e.User)
        //        .Include(e => e.ManagedLocations)
        //        .ToListAsync();
        //    //return await Query()
        //    //    .Include(e => e.Role)
        //    //    .Include(e => e.Workplace)
        //    //    .Include(e => e.User)
        //    //    .Include(e => e.ManagedLocations)
        //    //    .Where(predicate)
        //    //    .ExecuteAsync();
        //}

        public override async Task<IEnumerable<Employee>> GetByCriteriaAsync(Expression<Func<Employee, bool>> predicate)
        {
            if (_user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");

            if (!_user.HasPermission(AccessService.PermissionKey.ReadEmployee))
                throw new UnauthorizedAccessException("Немає дозволу читати працівників.");

            var employees = await _context.Employees
                .Where(predicate)
                .ToListAsync();

            if (!employees.Any())
                return employees;

            var employeeIds = employees.Select(e => e.ID).ToList();

            // Load related entities only if the user has permission

            //if (_user.HasPermission(AccessService.PermissionKey.ReadRole))
            //{
            //    await _context.Roles
            //        .Where(r => employees
            //        .Select(e => e.User.RoleID)
            //        .Contains(r.ID))
            //        .LoadAsync();
            //}

            if (_user.HasPermission(AccessService.PermissionKey.ReadLocation))
            {
                var workplaceIds = employees.Select(e => e.WorkplaceID).Distinct().ToList();
                await _context.Locations
                    .Where(w => workplaceIds.Contains(w.ID))
                    .LoadAsync();
            }

            if (_user.HasPermission(AccessService.PermissionKey.ReadUser))
            {
                var employeeIdsWithUsers = employees
                    .Where(e => e.User != null)
                    .Select(e => (uint?)e.ID)
                    .ToList();
                
                await _context.Users
                    .Where(u => employeeIdsWithUsers.Contains(u.PersonID))
                    .LoadAsync();
            }

            return employees;
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
                .Where(e => e.User != null && e.User.Role.Name == "Системний Адміністратор")
                .CountAsync();
        }
    }
}
