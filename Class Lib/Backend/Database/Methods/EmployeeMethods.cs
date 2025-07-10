using Microsoft.EntityFrameworkCore;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Class_Lib.Database.Repositories;
using Class_Lib.Backend.Database.Interfaces;

namespace Class_Lib.Backend.Person_related.Methods
{
    public class EmployeeMethods
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IRoleService _roleService;

        public EmployeeMethods(IEmployeeRepository employeeRepository, IRoleRepository roleRepository, IRoleService roleService)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            _roleService = roleService;
        }

        // Create
        public async Task AddAsync(User user, Employee employee)
        {
            await AccessService.ExecuteIfPermittedAsync(user, AccessService.PermissionKey.CreateEmployee, async () =>
            {
                await _employeeRepository.AddAsync(employee);
            });
        }

        // Read
        public async Task<IEnumerable<Employee>> GetByCriteriaAsync(User user, Expression<Func<Employee, bool>> filter)
        {
            await AccessService.ExecuteIfPermittedAsync(user, AccessService.PermissionKey.ReadEmployee, async () => { });
            return await _employeeRepository.GetByCriteriaAsync(filter);
        }

        // Update
        public async Task UpdateAsync(User user, Employee updatedEmployee)
        {
            await AccessService.ExecuteIfPermittedAsync(user, AccessService.PermissionKey.UpdateEmployee, async () =>
            {
                var existingEmployee = await _employeeRepository.GetByIdAsync(updatedEmployee.ID)
                    ?? throw new KeyNotFoundException("Працівника не знайдено.");

                existingEmployee.UpdateDetails(updatedEmployee);

                await _employeeRepository.UpdateAsync(existingEmployee);
            });
        }

        // Delete
        public async Task DeleteAsync(User user, Employee employeeToDelete)
        {
            await AccessService.ExecuteIfPermittedAsync(user, AccessService.PermissionKey.DeleteEmployee, async () =>
            {
                if (employeeToDelete.IsSystemAdministrator())
                {
                    var adminCount = await _employeeRepository.GetAdministratorCountAsync();
                    if (adminCount <= 1)
                        throw new InvalidOperationException("Не можна видалити останнього адміністратора системи.");
                }

                await _employeeRepository.DeleteAsync(employeeToDelete);
            });
        }

        // Promote to Manager
        public async Task PromoteToManagerAsync(User user, Employee employeeToUpdate, List<BaseLocation> managedLocations)
        {
            await AccessService.ExecuteIfPermittedAsync(user, AccessService.PermissionKey.UpdateEmployee, async () =>
            {
                var existingEmployee = await _employeeRepository.GetByIdAsync(employeeToUpdate.ID)
                    ?? throw new KeyNotFoundException("Працівника не знайдено.");
                var managerRole = await _roleRepository.GetRoleByNameAsync("Менеджер")
                    ?? throw new KeyNotFoundException("Роль менеджера не знайдена.");

                existingEmployee.PromoteToManager(managerRole, managedLocations);

                await _roleService.CachePermissionsAsync(existingEmployee.User);
                await _employeeRepository.UpdateAsync(existingEmployee);
            });
        }

        // Promote to Administrator
        public async Task PromoteToAdministratorAsync(User user, Employee employeeToUpdate)
        {
            await AccessService.ExecuteIfPermittedAsync(user, AccessService.PermissionKey.UpdateEmployee, async () =>
            {
                var existingEmployee = await _employeeRepository.GetByIdAsync(employeeToUpdate.ID)
                    ?? throw new KeyNotFoundException("Працівника не знайдено.");
                var adminRole = await _roleRepository.GetRoleByNameAsync("Системний Адміністратор")
                    ?? throw new KeyNotFoundException("Роль адміністратора не знайдена.");

                existingEmployee.PromoteToAdministrator(adminRole);

                await _roleService.CachePermissionsAsync(existingEmployee.User);
                await _employeeRepository.UpdateAsync(existingEmployee);
            });
        }
    }
}
