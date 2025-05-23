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
using static Class_Lib.Backend.Services.AccessService;

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

            if(!_user.HasPermission(PermissionKey.ReadDelivery))
                throw new UnauthorizedAccessException("Немає дозволу читати доставки.");

            var deliveries = await _context.Deliveries
                .Where(predicate)
                .ToListAsync();

            if (!deliveries.Any())
                return deliveries;

            var deliveryIds = deliveries.Select(d => d.ID).ToList();

            if (_user.HasPermission(PermissionKey.ReadPackage))
            {
                await _context.Packages
                    .Where(p => deliveryIds.Contains(p.Delivery.ID))
                    .LoadAsync();
                if(_user.HasPermission(PermissionKey.ReadContent))
                {
                    await _context.Contents
                        .Where(c => deliveryIds.Contains(c.Package.Delivery.ID))
                        .LoadAsync();
                }
            }

            if (_user.HasPermission(PermissionKey.ReadEmployee))
            {
                var senderIds = deliveries.Select(d => d.SenderID).Distinct().ToList();
                var receiverIds = deliveries.Select(d => d.ReceiverID).Distinct().ToList();

                await _context.Clients
                    .Where(p => senderIds.Contains(p.ID) || receiverIds.Contains(p.ID))
                    .LoadAsync();
            }

            if (_user.HasPermission(PermissionKey.ReadLocation))
            {
                var fromIds = deliveries.Select(d => d.SentFromID).Distinct().ToList();
                var toIds = deliveries.Select(d => d.SentToID).Distinct().ToList();

                var locationQuery = _context.Locations
                    .Where(l => fromIds.Contains(l.ID) || toIds.Contains(l.ID));

                if (_user.HasPermission(PermissionKey.ReadEmployee))
                {
                    locationQuery = locationQuery.Include(l => l.Staff);

                    if (_user.HasPermission(PermissionKey.ReadUser))
                    {
                        locationQuery = locationQuery.Include("Staff.User");

                        if (_user.HasPermission(PermissionKey.ReadRole))
                        {
                            locationQuery = locationQuery.Include("Staff.User.Role");
                        }
                    }
                }

                await locationQuery.LoadAsync();
            }

            return deliveries;
        }

    }
}

//if (_user.HasPermission(PermissionKey.ReadLocation))
//{
//    var fromIds = deliveries.Select(d => d.SentFromID).Distinct().ToList();
//    var toIds = deliveries.Select(d => d.SentToID).Distinct().ToList();

//    var canSeeStaff = _user.HasPermission(PermissionKey.ReadPerson);
//    var canSeeUser = _user.HasPermission(PermissionKey.ReadUser);
//    var canSeeRole = _user.HasPermission(PermissionKey.ReadRole);

//    //await _context.Locations
//    //    .Where(l => fromIds.Contains(l.ID) || toIds.Contains(l.ID))
//    //    .Include(l => l.Staff)
//    //    .LoadAsync();
//    // good on paper but doesnt load properly in practice due to AsNoTracking, cannot be used without it either
//    //var data = await _context.Locations
//    //    .Where(l => fromIds.Contains(l.ID) || toIds.Contains(l.ID))
//    //    .AsNoTracking()     // do not track changes to the entities
//    //    .Select(l => new {      // select
//    //        l.ID,               // id, geodata, loc type
//    //        l.GeoData,
//    //        l.LocationType,
//    //        Staff = (canSeeStaff && l.Staff != null)    // if can see staff,
//    //        ? l.Staff.Select(e => new {                 // select the following:
//    //            e.ID,                                   // ID, first name, surname
//    //            e.FirstName,
//    //            e.Surname,

//    //            User = (canSeeUser && e.User != null) ? new // if can see users, select the following:
//    //                {
//    //                    e.User.PersonID,                    // person ID (FK), username
//    //                    e.User.Username,                    // NOT PASSWORD

//    //                    Role = (canSeeRole && e.User.Role != null) ? new    // if can see role,
//    //                        {                                               // select the following:
//    //                            e.User.Role.ID,                             // role ID, role name
//    //                            e.User.Role.Name
//    //                        }
//    //                        : null          // cant see role, set to null
//    //                }
//    //                : null  // cant see user, set to null
//    //            }).ToList() // and drop into a list
//    //            : null // cant see staff, set to null
//    //        })
//    //    .ToListAsync();     // and drop into a list (asynchronously)


//}