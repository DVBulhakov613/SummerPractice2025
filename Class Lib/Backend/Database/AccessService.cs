using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Services
{
    public static class AccessService
    {
        public enum PermissionKey
        {
            // scope permissions
            LocalPermissions = 1,
            GlobalPermissions = 2,

            // package permissions
            ReadPackage = 10,
            CreatePackage = 11,
            UpdatePackage = 12,
            DeletePackage = 13,

            // package event permissions
            ReadEvent = 20,
            CreateEvent = 21,
            UpdateEvent = 22,
            DeleteEvent = 23,

            // content permissions
            ReadContent = 30,
            CreateContent = 31,
            UpdateContent = 32,
            DeleteContent = 33,

            // person permissions
            ReadPerson = 40,
            CreatePerson = 41,
            UpdatePerson = 42,
            DeletePerson = 43,

            // location permissions
            ReadLocation = 50,
            CreateLocation = 51,
            UpdateLocation = 52,
            DeleteLocation = 53,

            // report permissions
            ReadReport = 60,
            CreateReport = 61,
            UpdateReport = 62,
            DeleteReport = 63,

            // delivery vehicle permissions
            ReadDeliveryVehicle = 70,
            CreateDeliveryVehicle = 71,
            UpdateDeliveryVehicle = 72,
            DeleteDeliveryVehicle = 73,

            // database-related permissions
            ReadContentType = 80,
            CreateContentType = 81,
            UpdateContentType = 82,
            DeleteContentType = 83,

            ReadPackageStatus = 90,
            CreatePackageStatus = 91,
            UpdatePackageStatus = 92,
            DeletePackageStatus = 93,

            ReadPackageType = 100,
            CreatePackageType = 101,
            UpdatePackageType = 102,
            DeletePackageType = 103,

            ReadCountry = 110,
            CreateCountry = 111,
            UpdateCountry = 112,
            DeleteCountry = 113,

            // user account permissions
            ReadUser = 120,
            CreateUser = 121,
            UpdateUser = 122,
            DeleteUser = 123
        }

        public static bool CanPerformAction(Employee employee, int permissionKey)
        {
            return employee.CachedPermissions.Contains(permissionKey);
        }

        // assign default permissions based on role
        //public static async Task AssignRolePermissionsAsync(Employee employee, AppDbContext context)
        //{
        //    if (employee.RoleID == 0)
        //        throw new InvalidOperationException("Працівник не має ролі.");

        //    // Load permissions from the RolePermissions table
        //    var permissions = await context.RolePermissions
        //        .Where(rp => rp.RoleID == employee.RoleID)
        //        .Select(rp => rp.PermissionID)
        //        .ToListAsync();

        //    employee.Permissions = permissions.Select(p => (int)p).ToList();
        //}

        //public static void AssignRolePermissions(Employee employee, string role)
        //{
        //    if (RolePermissions.TryGetValue(role, out var permissions))
        //    {
        //        employee.Permissions = permissions.Select(p => (int)p).ToList();
        //    }
        //    else
        //    {
        //        throw new ArgumentException($"Роль '{role}' не існує.");
        //    }
        //}

        public static PermissionKey? GetReadPermissionForType(Type entityType)
        {
            return entityType.Name switch
            {
                nameof(Package) => PermissionKey.ReadPackage,
                nameof(PackageEvent) => PermissionKey.ReadEvent,
                nameof(Content) => PermissionKey.ReadContent,
                nameof(Person) => PermissionKey.ReadPerson,
                nameof(BaseLocation) => PermissionKey.ReadLocation,
                //nameof(Report) => PermissionKey.ReadReport,
                //nameof(DeliveryVehicle) => PermissionKey.ReadDeliveryVehicle,
                nameof(ContentType) => PermissionKey.ReadContentType,
                //nameof(PackageStatus) => PermissionKey.ReadPackageStatus,
                nameof(PackageType) => PermissionKey.ReadPackageType,
                //nameof(Country) => PermissionKey.ReadCountry,
                nameof(User) => PermissionKey.ReadUser,

                _ => null
            };
        }
    }

    /// <summary>
    /// Represents a role in the system.
    /// </summary>
    public class Role
    {
        public uint ID { get; set; }
        public string Name { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }

    /// <summary>
    /// Represents a many-to-many relationship between roles and permissions.
    /// </summary>
    public class RolePermission
    {
        public uint RoleID { get; set; }
        public uint PermissionID { get; set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }

    /// <summary>
    /// Represents a permission in the system.
    /// </summary>
    public class  Permission
    {
        public uint ID { get; set; }
        public string Name { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }



    public class RoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CachePermissionsAsync(Employee employee)
        {
            if (employee.RoleID == 0)
                throw new InvalidOperationException("Працівник не має ролі.");

            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleID == employee.RoleID)
                .Select(rp => rp.PermissionID)
                .ToListAsync();

            employee.CachedPermissions = permissions.Select(p => (int)p).ToList();
        }
    }
}
