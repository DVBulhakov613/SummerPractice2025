using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Location_related.Methods
{
    public class LocationMethods
    {
        private readonly AppDbContext _context;

        public LocationMethods(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddBaseLocationAsync(Person user, BaseLocation location)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "CreateLocation"))
            {
                throw new UnauthorizedAccessException("Немає дозволу створювати локації.");
            }

            _context.Locations.Add(location);

            await _context.SaveChangesAsync();
        }

        public async Task EditBaseLocationAsync(Person user, BaseLocation location)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "UpdateLocation"))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати локації.");
            }

            _context.Locations.Update(location);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteBaseLocationAsync(Person user, BaseLocation location)
        {
            if (!AccessService.CanPerformAction(user.GetType(), "DeleteLocation"))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти локації.");
            }

            _context.Locations.Remove(location);

            await _context.SaveChangesAsync();
        }
    }

}
