using Microsoft.EntityFrameworkCore;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Class_Lib.Database.Repositories;

namespace Class_Lib.Backend.Person_related.Methods
{
    public class EmployeeMethods
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeMethods(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }


        // Create
        public async Task AddEmployeeAsync(Employee user, Employee employee)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "CreatePerson"))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення працівника.");
            }

            await _employeeRepository.AddAsync(employee);
        }

        // Read
        public async Task<IEnumerable<Employee>> GetEmployeesByCriteriaAsync(Employee user, Expression<Func<Employee, bool>> filter)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "ReadPerson"))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду працівників.");
            }

            return await _employeeRepository.Query()
                .Where(filter)
                .ExecuteAsync();
        }

        // Update
        public async Task UpdateEmployeeAsync(Employee user, Employee updatedEmployee)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "UpdatePerson"))
            {
                throw new UnauthorizedAccessException("Немає доступу до оновлення працівника.");
            }

            var existingEmployee = await _employeeRepository.GetByIdAsync(updatedEmployee.ID);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("Працівника не знайдено.");
            }

            existingEmployee.Position = updatedEmployee.Position;
            existingEmployee.Workplace = updatedEmployee.Workplace;
            existingEmployee.PhoneNumber = updatedEmployee.PhoneNumber;
            existingEmployee.Email = updatedEmployee.Email;

            await _employeeRepository.UpdateAsync(existingEmployee);
        }

        // Delete
        public async Task DeleteEmployeeAsync(Employee user, Employee employee)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "DeletePerson"))
            {
                throw new UnauthorizedAccessException("Немає доступу до видалення працівника.");
            }

            if (employee is Administrator)
            {
                // Use the repository to count administrators
                var adminCount = await _employeeRepository.GetAdministratorCountAsync();
                if (adminCount <= 1)
                {
                    throw new InvalidOperationException("Не можна видалити останнього адміністратора системи.");
                }
            }

            await _employeeRepository.DeleteAsync(employee);
        }

        // Employee-Specific Methods
        public async Task PromoteToManagerAsync(Employee user, Employee employee, List<BaseLocation> managedLocations)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "UpdatePerson"))
            {
                throw new UnauthorizedAccessException("Немає доступу до підвищення працівника.");
            }

            var existingEmployee = await _employeeRepository.GetByIdAsync(employee.ID);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("Працівника не знайдено.");
            }

            var manager = new Manager(
                existingEmployee.FirstName,
                existingEmployee.Surname,
                existingEmployee.PhoneNumber,
                existingEmployee.Email,
                existingEmployee.Position,
                existingEmployee.Workplace
            )
            {
                ManagedLocations = managedLocations
            };

            await _employeeRepository.DeleteAsync(existingEmployee);
            await _employeeRepository.AddAsync(manager);
        }

        public async Task PromoteToAdministratorAsync(Employee user, Employee employee)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "UpdatePerson"))
            {
                throw new UnauthorizedAccessException("Немає доступу до підвищення працівника.");
            }

            var existingEmployee = await _employeeRepository.GetByIdAsync(employee.ID);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("Працівника не знайдено.");
            }

            var administrator = new Administrator(
                existingEmployee.FirstName,
                existingEmployee.Surname,
                existingEmployee.PhoneNumber,
                existingEmployee.Email,
                existingEmployee.Position,
                existingEmployee.Workplace
            );

            await _employeeRepository.DeleteAsync(existingEmployee);
            await _employeeRepository.AddAsync(administrator);
        }
    }
}
