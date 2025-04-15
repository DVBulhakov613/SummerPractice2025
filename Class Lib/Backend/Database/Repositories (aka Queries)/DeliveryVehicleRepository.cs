using Class_Lib.Backend.Delivery_vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Database.Repositories
{
    public class DeliveryVehicleRepository : Repository<DeliveryVehicle>
    {
        public DeliveryVehicleRepository(AppDbContext context) : base(context)
        {
        }


    }
}
