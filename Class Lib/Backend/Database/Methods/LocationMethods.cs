using Class_Lib.Backend.Database;
using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Person_related.Methods;
using Class_Lib.Backend.Services;
using Class_Lib.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Location_related.Methods
{
    public class LocationMethods
    {
        private readonly LocationRepository _locationRepository;
        private readonly EmployeeMethods _employeeMethods;

        public LocationMethods(LocationRepository locationRepository, EmployeeMethods employeeMethods)
        {
            _locationRepository = locationRepository;
            _employeeMethods = employeeMethods;
        }

        // Create
        public async Task AddAsync(User user, BaseLocation location)
        {
            if (!user.HasPermission(AccessService.PermissionKey.CreateLocation)) throw new UnauthorizedAccessException("Немає дозволу створювати локації.");

            await _locationRepository.AddAsync(location);
        }

        // Read
        public async Task<IEnumerable<BaseLocation>> GetByCriteriaAsync(User user, Expression<Func<BaseLocation, bool>> filter)
        {
            if (!user.HasPermission(AccessService.PermissionKey.ReadLocation)) throw new UnauthorizedAccessException("Немає доступу до перегляду локацій.");

            var locations = (await _locationRepository.GetByCriteriaAsync(filter)).ToList();

            if (user.HasPermission(AccessService.PermissionKey.ReadPerson))
            {
                var locationIds = locations.Select(l => l.ID).ToList();

                var allEmployees = await _employeeMethods.GetByCriteriaAsync(
                    user, e => locationIds.Contains(e.Workplace.ID)
                );

                var employeesByLocation = allEmployees
                    .GroupBy(e => e.Workplace.ID)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var location in locations)
                {
                    location.Staff = employeesByLocation.TryGetValue(location.ID, out var staff) ? staff : new List<Employee>();
                }
            }
            else
                foreach (var location in locations)
                    location.Staff = null;

            return locations;
        }



        // Update
        public async Task UpdateAsync(User user, BaseLocation location)
        {
            if (!user.HasPermission(AccessService.PermissionKey.UpdateLocation))
            {
                throw new UnauthorizedAccessException("Немає дозволу змінювати локації.");
            }

            await _locationRepository.UpdateAsync(location);
        }

        // Delete
        public async Task DeleteAsync(User user, BaseLocation location)
        {
            if (!user.HasPermission(AccessService.PermissionKey.DeleteLocation))
            {
                throw new UnauthorizedAccessException("Немає дозволу видаляти локації.");
            }

            await _locationRepository.DeleteAsync(location);
        }
    }
}
