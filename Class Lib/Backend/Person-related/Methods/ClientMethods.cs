using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Person_related.Methods
{
    public class ClientMethods
    {
        private readonly ClientRepository _clientRepository;

        public ClientMethods(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // Create
        public async Task AddClientAsync(Employee user, Client client)
        {
            if (!user.HasPermission(AccessService.PermissionKey.CreatePerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення клієнта.");
            }

            await _clientRepository.AddAsync(client);
        }

        // Read
        public async Task<IEnumerable<Client>> GetClientsByCriteriaAsync(Employee user, Expression<Func<Client, bool>> filter)
        {
            if (!user.HasPermission(AccessService.PermissionKey.ReadPerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду клієнтів.");
            }

            return await _clientRepository.Query()
                .Where(filter)
                .ExecuteAsync();
        }

        // Delete
        public async Task DeleteClientAsync(Employee user, Client client)
        {
            if (!user.HasPermission(AccessService.PermissionKey.DeletePerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до видалення клієнта.");
            }

            await _clientRepository.DeleteAsync(client);
        }
    }
}
