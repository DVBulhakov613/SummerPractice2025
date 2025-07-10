using Microsoft.EntityFrameworkCore;

namespace Class_Lib.Backend.Database.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        // by ID
        public Task<IEnumerable<Employee>> GetByIDAsync(uint id);

        // by workplace ID
        public Task<IEnumerable<Employee>> GetByWorkplaceIdAsync(uint workplaceId);


        // by first name
        public Task<IEnumerable<Employee>> GetByFirstNameAsync(string firstName);


        // by last name
        public Task<IEnumerable<Employee>> GetByLastNameAsync(string lastName);


        // by full name
        public Task<IEnumerable<Employee>> GetByFullNameAsync(string fullName);


        // administrator count (for deletion restriction)
        public Task<int> GetAdministratorCountAsync();

    }
}