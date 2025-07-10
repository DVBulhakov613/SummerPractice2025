using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Person_related;
using System;
using System.Collections.Generic;
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
            ReadEmployee = 40,
            CreateEmployee = 41,
            UpdateEmployee = 42,
            DeleteEmployee = 43,

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
            DeleteUser = 123,

            ReadDelivery = 130,
            CreateDelivery = 131,
            UpdateDelivery = 132,
            DeleteDelivery = 133,

            ReadRole = 140,
            CreateRole = 141,
            UpdateRole = 142,
            DeleteRole = 143,

            ReadClient = 150,
            CreateClient = 151,
            UpdateClient = 152,
            DeleteClient = 153,

            ReadRolePermissions = 160,
            CreateRolePermissions = 161,
            UpdateRolePermissions = 162,
            DeleteRolePermissions = 163,

            ReadPermission = 170,
            CreatePermission = 171,
            UpdatePermission = 172,
            DeletePermission = 173,
        }

        public static bool CanPerformAction(User employee, int permissionKey)
        {
            return employee.CachedPermissions.Contains(permissionKey);
        }

        public static bool CanPerformAction(User employee, PermissionKey permissionKey)
        {
            return CanPerformAction(employee, (int)permissionKey);
        }

        public static async Task ExecuteIfPermittedAsync(User user, PermissionKey permission, Func<Task> action)
        {
            if (user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");
            if (!user.HasPermission(permission))
                throw new UnauthorizedAccessException("Немає дозволу на цю дію.");

            await action();
        }

        public static void ExecuteIfPermitted(User user, PermissionKey permission, Action action)
        {
            if (user == null)
                throw new UnauthorizedAccessException("Користувач не авторизований.");
            if (!user.HasPermission(permission))
                throw new UnauthorizedAccessException("Немає дозволу на цю дію.");

            action();
        }

        public static PermissionKey? GetReadPermissionForType(Type entityType)
        {
            return entityType.Name switch
            {
                nameof(Package) => PermissionKey.ReadPackage,
                nameof(PackageEvent) => PermissionKey.ReadEvent,
                nameof(Content) => PermissionKey.ReadContent,
                nameof(Employee) => PermissionKey.ReadEmployee,
                nameof(BaseLocation) => PermissionKey.ReadLocation,
                //nameof(Report) => PermissionKey.ReadReport,
                //nameof(DeliveryVehicle) => PermissionKey.ReadDeliveryVehicle,
                nameof(ContentType) => PermissionKey.ReadContentType,
                //nameof(PackageStatus) => PermissionKey.ReadPackageStatus,
                nameof(PackageType) => PermissionKey.ReadPackageType,
                //nameof(Country) => PermissionKey.ReadCountry,
                nameof(User) => PermissionKey.ReadUser,
                nameof(Delivery) => PermissionKey.ReadDelivery,
                nameof(Permission) => PermissionKey.ReadPermission,
                nameof(Role) => PermissionKey.ReadRole,
                nameof(Client) => PermissionKey.ReadClient,
                nameof(RolePermission) => PermissionKey.ReadRolePermissions,

                _ => null
            };
        }
    }
}
