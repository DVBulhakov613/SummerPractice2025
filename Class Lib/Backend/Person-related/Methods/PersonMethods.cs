using Microsoft.EntityFrameworkCore;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Class_Lib.Database.Repositories;

namespace Class_Lib.Backend.Person_related.Methods
{
    public class PersonMethods
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly ClientRepository _clientRepository;
        private readonly AppDbContext _context;

        public PersonMethods(EmployeeRepository employeeRepository, ClientRepository clientRepository)
        {
            _employeeRepository = employeeRepository;
            _clientRepository = clientRepository;
        }

        public async Task AddEmployeeAsync(Person user, Employee employee)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "CreatePerson"))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення працівника.");
            }
            
            await _employeeRepository.AddAsync(employee);
        }

        public async Task AddClientAsync(Person user, Client client)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "CreatePerson"))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення клієнта.");
            }

            await _clientRepository.AddAsync(client);
        }

        public async Task DeleteAdministratorAsync(Person user, Administrator admin)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "DeletePerson"))
            {
                throw new UnauthorizedAccessException("Нема дозволення видалити Адміністратора.");
            }

            var adminCount = await _context.Employees.OfType<Administrator>().CountAsync();
            if (adminCount <= 1)
            {
                throw new InvalidOperationException("Не можна видалити останнього адміністратора системи.");
            }

            await _employeeRepository.DeleteAsync(admin);
        }
    }
}
