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
        public async Task AddAsync(Employee user, Client client)
        {
            if (!user.HasPermission(AccessService.PermissionKey.CreatePerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до створення клієнта.");
            }

            await _clientRepository.AddAsync(client);
        }

        // Read
        public async Task<IEnumerable<Client>> GetByCriteriaAsync(Employee user, Expression<Func<Client, bool>> filter)
        {
            if (!user.HasPermission(AccessService.PermissionKey.ReadPerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду клієнтів.");
            }

            return await _clientRepository.Query()
                .Where(filter)
                .ExecuteAsync();
        }

        // Update
        public async Task UpdateAsync(Employee user, Client updatedClient)
        {
            if (!user.HasPermission(AccessService.PermissionKey.UpdatePerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до оновлення клієнта.");
            }
            var existingClient = await _clientRepository.GetByIdAsync(updatedClient.ID);
            if (existingClient == null)
            {
                throw new KeyNotFoundException("Клієнт не знайдений.");
            }
            await _clientRepository.UpdateAsync(updatedClient);
        }

        // Delete
        public async Task DeleteAsync(Employee user, Client client)
        {
            if (!user.HasPermission(AccessService.PermissionKey.DeletePerson))
            {
                throw new UnauthorizedAccessException("Немає доступу до видалення клієнта.");
            }

            await _clientRepository.DeleteAsync(client);
        }
    }
}
