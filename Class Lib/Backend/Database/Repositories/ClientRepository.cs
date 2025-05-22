using Class_Lib.Backend.Database;
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
    public class ClientRepository : Repository<Client>
    {
        public ClientRepository(AppDbContext context, User user) : base(context, user) { }

        public override async Task<IEnumerable<Client>> GetByCriteriaAsync(Expression<Func<Client, bool>> predicate)
        {
            if (_user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");

            if (!_user.HasPermission(AccessService.PermissionKey.ReadClient))
                throw new UnauthorizedAccessException("Немає дозволу читати клієнтів.");

            var clients = await _context.Clients
                .Where(predicate)
                .ToListAsync();

            if(!clients.Any())
                return clients;

            var clientIds = clients.Select(c => c.ID).ToList();

            // Load related entities only if the user has permission

            // i know that this is not the best way to do it but it's the best i can do at the moment
            if (_user.HasPermission(AccessService.PermissionKey.ReadDelivery))
            {
                await _context.Deliveries
                    .Where(d => clientIds.Contains(d.SenderID) || clientIds.Contains(d.ReceiverID))
                    .LoadAsync();

                if(_user.HasPermission(AccessService.PermissionKey.ReadPackage))
                {
                    await _context.Packages
                        .Where(p => clientIds.Contains(p.Delivery.SenderID) || clientIds.Contains(p.Delivery.ReceiverID))
                        .LoadAsync();

                    if(_user.HasPermission(AccessService.PermissionKey.ReadContent))
                    {
                        await _context.Contents
                            .Where(c => clientIds.Contains(c.Package.Delivery.SenderID) || clientIds.Contains(c.Package.Delivery.ReceiverID))
                            .LoadAsync();
                    }
                }
            }

            return clients;
        }

        /*
            
        public override async Task<IEnumerable<Delivery>> GetByCriteriaAsync(Expression<Func<Delivery, bool>> predicate)
        {
            if (_user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");

            // Always fetch deliveries first
            if(!_user.User.HasPermission(AccessService.PermissionKey.ReadDelivery))
                throw new UnauthorizedAccessException("Немає дозволу читати доставки.");

            var deliveries = await _context.Deliveries
                .Where(predicate)
                .ToListAsync();

            if (!deliveries.Any())
                return deliveries;

            var deliveryIds = deliveries.Select(d => d.ID).ToList();

            // Load related entities only if the user has permission

            if (_user.HasPermission(AccessService.PermissionKey.ReadPackage))
            {
                await _context.Packages
                    .Where(p => deliveryIds.Contains(p.Delivery.ID)) // assumes Package has a DeliveryId FK
                    .LoadAsync();
            }

            if (_user.HasPermission(AccessService.PermissionKey.ReadPerson))
            {
                var senderIds = deliveries.Select(d => d.SenderID).Distinct().ToList();
                var receiverIds = deliveries.Select(d => d.ReceiverID).Distinct().ToList();

                await _context.Clients
                    .Where(p => senderIds.Contains(p.ID) || receiverIds.Contains(p.ID))
                    .LoadAsync();
            }

            if (_user.HasPermission(AccessService.PermissionKey.ReadLocation))
            {
                var fromIds = deliveries.Select(d => d.SentFromID).Distinct().ToList();
                var toIds = deliveries.Select(d => d.SentToID).Distinct().ToList();

                await _context.Locations
                    .Where(l => fromIds.Contains(l.ID) || toIds.Contains(l.ID))
                    .Include(l => l.Staff)
                    .LoadAsync();
            }

            return deliveries;
        }

         */

        // by ID
        public async Task<IEnumerable<Client>> GetClientsByIDAsync(uint id)
        {
            return await GetByCriteriaAsync(p => p.ID == id);
        }

        // by first name
        public async Task<IEnumerable<Client>> GetClientsByFirstNameAsync(string firstName)
        {
            return await GetByCriteriaAsync(p => p.FirstName.ToLower() == firstName.ToLower());
        }

        // by last name
        public async Task<IEnumerable<Client>> GetClientsByLastNameAsync(string lastName)
        {
            return await GetByCriteriaAsync(p => p.Surname.ToLower() == lastName.ToLower());
        }

        // by full name
        public async Task<IEnumerable<Client>> GetClientsByFullNameAsync(string fullName)
        {
            return await GetByCriteriaAsync(p => p.FirstName.ToLower() + " " + p.Surname.ToLower() == fullName.ToLower());
        }
    }
}
