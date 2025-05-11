using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Database.Repositories
{
    public class DeliveryRepository : Repository<Delivery>
    {
        public DeliveryRepository(AppDbContext context, User user) : base(context, user)
        {
        }

        public override async Task<IEnumerable<Delivery>> GetByCriteriaAsync(Expression<Func<Delivery, bool>> predicate)
        {
            if (_user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");

            // Always fetch deliveries first
            if(!_user.HasPermission(AccessService.PermissionKey.ReadDelivery))
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
                    .Where(p => deliveryIds.Contains(p.Delivery.ID))
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

    }
}
