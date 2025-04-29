using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Location_related.Methods
{
    public class LocationMethods
    {
        private readonly LocationRepository _locationRepository;

        public LocationMethods(LocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        // Create
        public async Task AddLocationAsync(Employee user, BaseLocation location)
        {
            if (!user.HasPermission(AccessService.PermissionKey.CreateLocation))
            {
                throw new UnauthorizedAccessException("Немає дозволу створювати локації.");
            }

            await _locationRepository.AddAsync(location);
        }

        // Read
        public async Task<IEnumerable<BaseLocation>> GetLocationsByCustomCriteriaAsync(Employee user, Expression<Func<BaseLocation, bool>> filter)
        {
            if (!user.HasPermission(AccessService.PermissionKey.ReadLocation))
            {
                throw new UnauthorizedAccessException("Немає доступу до перегляду локацій.");
            }

            return await _locationRepository.Query()
                .Where(filter)
                .ExecuteAsync();
        }

        // Update
        public async Task UpdateLocationAsync(Employee user, BaseLocation location)
        {
            if (!user.HasPermission(AccessService.PermissionKey.UpdateLocation))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати локації.");
            }

            await _locationRepository.UpdateAsync(location);
        }

        // Delete
        public async Task DeleteLocationAsync(Employee user, BaseLocation location)
        {
            if (!user.HasPermission(AccessService.PermissionKey.DeleteLocation))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти локації.");
            }

            await _locationRepository.DeleteAsync(location);
        }
    }
}
