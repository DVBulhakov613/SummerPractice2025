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

        public async Task<IEnumerable<Delivery>> GetByCriteriaAsync(Employee user, Expression<Func<Delivery, bool>> predicate)
        {
            if (predicate == null) { throw new ArgumentNullException(nameof(predicate), "Пустий фільтр пошуку."); }
            if (!user.HasPermission(AccessService.PermissionKey.ReadDelivery))
            {
                throw new UnauthorizedAccessException("Немає дозволу читати доставки.");
            }

            return await _deliveryRepository.GetByCriteriaAsync(predicate);
        }
    }
}
