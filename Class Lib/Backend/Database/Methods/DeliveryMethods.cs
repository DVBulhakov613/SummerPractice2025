using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Package_related.Methods
{
    public class DeliveryMethods
    {
        private readonly DeliveryRepository _deliveryRepository;

        public DeliveryMethods(DeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task AddAsync(User user, Delivery delivery)
        {
            if (delivery == null) { throw new ArgumentNullException(nameof(delivery), "Параметр доставки не може бути пустим."); }
            if (!user.HasPermission(AccessService.PermissionKey.CreateDelivery))
            {
                throw new UnauthorizedAccessException("Немає дозволу створювати доставки.");
            }
            await _deliveryRepository.AddAsync(delivery);
        }

        public async Task<IEnumerable<Delivery>> GetByCriteriaAsync(User user, Expression<Func<Delivery, bool>> predicate)
        {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate), "Пустий фільтр пошуку."); }
            if (!user.HasPermission(AccessService.PermissionKey.ReadDelivery))
            {
                throw new UnauthorizedAccessException("Немає дозволу читати доставки.");
            }

            return await _deliveryRepository.GetByCriteriaAsync(predicate);
        }

        public async Task UpdateAsync(User user, Delivery delivery)
        {
            if (delivery == null) { throw new ArgumentNullException(nameof(delivery), "Параметр доставки не може бути пустим."); }
            if (!user.HasPermission(AccessService.PermissionKey.UpdateDelivery))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати доставки.");
            }
            await _deliveryRepository.UpdateAsync(delivery);
        }

        public async Task DeleteAsync(User user, Delivery delivery)
        {
            if (delivery == null) { throw new ArgumentNullException(nameof(delivery), "Параметр доставки не може бути пустим."); }
            if (!user.HasPermission(AccessService.PermissionKey.DeleteDelivery))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти доставки.");
            }
            await _deliveryRepository.DeleteAsync(delivery);
        }
    }
}
