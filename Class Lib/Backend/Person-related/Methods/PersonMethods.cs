//using Microsoft.EntityFrameworkCore;
//using Class_Lib.Backend.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Class_Lib.Database.Repositories;
//using System.Linq.Expressions;

//namespace Class_Lib.Backend.Person_related.Methods
//{
//    public class PersonMethods
//    {
//        private readonly EmployeeRepository _employeeRepository;
//        private readonly ClientRepository _clientRepository;
//        private readonly AppDbContext _context;

//        public PersonMethods(EmployeeRepository employeeRepository, ClientRepository clientRepository)
//        {
//            _employeeRepository = employeeRepository;
//            _clientRepository = clientRepository;
//        }

//        // Create
//        public async Task AddEmployeeAsync(Person user, Employee employee)
//        {
//            if (!AccessService.CanPerformAction(user.GetType(), "CreatePerson"))
//            {
//                throw new UnauthorizedAccessException("Немає доступу до створення працівника.");
//            }
            
//            await _employeeRepository.AddAsync(employee);
//        }

//        // Read
//        public async Task<IEnumerable<Employee>> GetEmployeesByCriteriaAsync(Person user, Expression<Func<Employee, bool>> filter)
//        {
//            if (!AccessService.CanPerformAction(user.GetType(), "ReadPerson"))
//            {
//                throw new UnauthorizedAccessException("Немає доступу до перегляду працівників.");
//            }

//            return await _employeeRepository.Query()
//                .Where(filter)
//                .ExecuteAsync();
//        }


//        // Update
//        public async Task UpdateEmployeeAsync(Person user, Employee updatedEmployee)
//        {
//            if (!AccessService.CanPerformAction(user.GetType(), "UpdatePerson"))
//            {
//                throw new UnauthorizedAccessException("Немає доступу до оновлення працівника.");
//            }

//            var existingEmployee = await _employeeRepository.GetByIdAsync(updatedEmployee.ID);
//            if (existingEmployee == null)
//            {
//                throw new KeyNotFoundException("Працівника не знайдено.");
//            }

//            existingEmployee.Position = updatedEmployee.Position;
//            existingEmployee.Workplace = updatedEmployee.Workplace;
//            existingEmployee.PhoneNumber = updatedEmployee.PhoneNumber;
//            existingEmployee.Email = updatedEmployee.Email;

//            await _employeeRepository.UpdateAsync(existingEmployee);
//        }

//        // Delete
//        public async Task DeleteEmployeeAsync(Person user, Employee employee)
//        {
//            if (!AccessService.CanPerformAction(user.GetType(), "DeletePerson"))
//            {
//                throw new UnauthorizedAccessException("Немає доступу до видалення працівника.");
//            }

//            if (employee is Administrator)
//            {
//                // cannot delete the last administrator of the system
//                var adminCount = await _context.Employees.OfType<Administrator>().CountAsync();
//                if (adminCount <= 1)
//                {
//                    throw new InvalidOperationException("Не можна видалити останнього адміністратора системи.");
//                }
//            }

//            await _employeeRepository.DeleteAsync(employee);
//        }

        
        
//        // employee-specific below


//        public async Task PromoteToManagerAsync(Person user, Employee employee, List<BaseLocation> managedLocations)
//        {
//            if (!AccessService.CanPerformAction(user.GetType(), "UpdatePerson"))
//            {
//                throw new UnauthorizedAccessException("Немає доступу до підвищення працівника.");
//            }

//            var existingEmployee = await _employeeRepository.GetByIdAsync(employee.ID);
//            if (existingEmployee == null)
//            {
//                throw new KeyNotFoundException("Працівника не знайдено.");
//            }

//            var manager = new Manager(
//                existingEmployee.FirstName,
//                existingEmployee.Surname,
//                existingEmployee.PhoneNumber,
//                existingEmployee.Email,
//                existingEmployee.Position,
//                existingEmployee.Workplace
//            )
//            {
//                ManagedLocations = managedLocations
//            };

//            await _employeeRepository.DeleteAsync(existingEmployee);
//            await _employeeRepository.AddAsync(manager);
//        }

//        //
//        public async Task PromoteToAdministratorAsync(Person user, Employee employee)
//        {
//            if (!AccessService.CanPerformAction(user.GetType(), "UpdatePerson"))
//            {
//                throw new UnauthorizedAccessException("Немає доступу до підвищення працівника.");
//            }

//            var existingEmployee = await _employeeRepository.GetByIdAsync(employee.ID);
//            if (existingEmployee == null)
//            {
//                throw new KeyNotFoundException("Працівника не знайдено.");
//            }

//            var administrator = new Administrator(
//                existingEmployee.FirstName,
//                existingEmployee.Surname,
//                existingEmployee.PhoneNumber,
//                existingEmployee.Email,
//                existingEmployee.Position,
//                existingEmployee.Workplace
//            );

//            await _employeeRepository.DeleteAsync(existingEmployee);
//            await _employeeRepository.AddAsync(administrator);
//        }

//        public async Task AddClientAsync(Person user, Client client)
//        {
//            if (!AccessService.CanPerformAction(user.GetType(), "CreatePerson"))
//            {
//                throw new UnauthorizedAccessException("Немає доступу до створення клієнта.");
//            }

//            await _clientRepository.AddAsync(client);
//        }

//        public async Task DeleteClientAsync(Person user, Client client)
//        {
//            if (!AccessService.CanPerformAction(user.GetType(), "DeletePerson"))
//            {
//                throw new UnauthorizedAccessException("Немає доступу до видалення клієнта.");
//            }
//            await _clientRepository.DeleteAsync(client);
//        }
//    }
//}
