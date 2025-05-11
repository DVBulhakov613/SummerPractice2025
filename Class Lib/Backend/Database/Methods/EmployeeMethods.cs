using Microsoft.EntityFrameworkCore;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Class_Lib.Database.Repositories;
using System.Reflection.Metadata.Ecma335;
using Class_Lib.Backend.Database.Repositories;

namespace Class_Lib.Backend.Person_related.Methods
{
    public class EmployeeMethods
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly RoleRepository _roleRepository;
        private readonly RoleService _roleService;

        public EmployeeMethods(EmployeeRepository employeeRepository, RoleRepository roleRepository, RoleService roleService)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            _roleService = roleService;
        }


        // Create
        public async Task AddAsync(User user, Employee employee)
        {
            if (!user.HasPermission(AccessService.PermissionKey.CreatePerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення працівника.");
            }

            await _employeeRepository.AddAsync(employee);
        }


        // Read
        public async Task<IEnumerable<Employee>> GetByCriteriaAsync(User user, Expression<Func<Employee, bool>> filter)
        {
            if (!user.HasPermission(AccessService.PermissionKey.ReadPerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду працівників.");
            }

            return await _employeeRepository.GetByCriteriaAsync(filter);
        }

        // Update
        public async Task UpdateAsync(User user, Employee updatedEmployee)
        {
            if (!user.HasPermission(AccessService.PermissionKey.UpdatePerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до оновлення працівника.");
            }

            var existingEmployee = await _employeeRepository.GetByIdAsync(updatedEmployee.ID);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("Працівника не знайдено.");
            }

            existingEmployee.Workplace = updatedEmployee.Workplace;
            existingEmployee.PhoneNumber = updatedEmployee.PhoneNumber;
            existingEmployee.Email = updatedEmployee.Email;

            await _employeeRepository.UpdateAsync(existingEmployee);
        }

        // Delete
        public async Task DeleteAsync(User user, Employee employeeToDelete)
        {
            if (!user.HasPermission(AccessService.PermissionKey.DeletePerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до видалення працівника.");
            }

            if(employeeToDelete.User == null || employeeToDelete.User.Role == null)
            {
                throw new ArgumentNullException("Роль працівника не знайдена.");
            }

            if (employeeToDelete.User.Role.Name == "Системний Адміністратор")
            {
                // use the repository to count administrators
                var adminCount = await _employeeRepository.GetAdministratorCountAsync();
                if (adminCount <= 1)
                {
                    throw new InvalidOperationException("Не можна видалити останнього адміністратора системи.");
                }
            }

            await _employeeRepository.DeleteAsync(employeeToDelete);
        }

        // Employee-Specific Methods

        // switches out permission list to manager's
        public async Task PromoteToManagerAsync(User user, Employee employeeToUpdate, List<BaseLocation> managedLocations)
        {
            if (!user.HasPermission(AccessService.PermissionKey.UpdatePerson))
                throw new UnauthorizedAccessException("Немає доступу до підвищення працівника.");

            var existingEmployee = await _employeeRepository.GetByIdAsync(employeeToUpdate.ID);
            if (existingEmployee == null)
                throw new KeyNotFoundException("Працівника не знайдено.");

            var managerRole = await _roleRepository.GetRoleByNameAsync("Менеджер");
            if (managerRole == null)
                throw new KeyNotFoundException("Роль менеджера не знайдена.");

            if (existingEmployee.User != null && existingEmployee.User.RoleID == managerRole.ID)
                throw new ArgumentException("Працівник вже є менеджером.");

            if (existingEmployee.User == null)
                existingEmployee.User = new User($"{existingEmployee.FirstName}.{existingEmployee.Surname}", PasswordHelper.HashPassword("defaultpassword"), managerRole, existingEmployee);

            existingEmployee.ManagedLocations = managedLocations;

            await _roleService.CachePermissionsAsync(existingEmployee.User);

            await _employeeRepository.UpdateAsync(existingEmployee);
        }


        // switches out permission list to administrator's
        public async Task PromoteToAdministratorAsync(User user, Employee employeeToUpdate)
        {
            if (!user.HasPermission(AccessService.PermissionKey.UpdatePerson))
                throw new UnauthorizedAccessException("Немає доступу до підвищення працівника.");

            var existingEmployee = await _employeeRepository.GetByIdAsync(employeeToUpdate.ID);
            if (existingEmployee == null)
                throw new KeyNotFoundException("Працівника не знайдено.");

            var adminRole = await _roleRepository.GetRoleByNameAsync("Системний Адміністратор");
            if (adminRole == null)
                throw new KeyNotFoundException("Роль адміністратора не знайдена.");

            if (existingEmployee.User != null && existingEmployee.User.RoleID == adminRole.ID)
                throw new ArgumentException("Працівник вже є адміністратором.");

            if (existingEmployee.User == null)
                existingEmployee.User = new User($"{existingEmployee.FirstName}.{existingEmployee.Surname}", PasswordHelper.HashPassword("defaultpassword"), adminRole, existingEmployee);

            existingEmployee.User.RoleID = adminRole.ID;

            await _roleService.CachePermissionsAsync(existingEmployee.User);

            await _employeeRepository.UpdateAsync(existingEmployee);
        }
    }
}
