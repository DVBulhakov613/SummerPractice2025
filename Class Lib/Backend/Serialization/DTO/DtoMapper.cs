using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.enums;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Class_Lib.Backend.Services.AccessService;

namespace Class_Lib.Backend.Serialization.DTO
{
    public static class DtoMapper
    {
        //public static UserDTO ToDto(User user)
        //{
        //    ArgumentNullException.ThrowIfNull(nameof(User));
        //    UserDTO uDto = new()
        //    {
        //        Username = user.Username,
        //        Role = new RoleDTO
        //        {
        //            ID = user.Role.ID,
        //            Name = user.Role.Name,
        //        }
        //    };

        //    if (user.Employee != null)
        //    {
        //        var emp = user.Employee;
        //        uDto.ID = user.Employee.ID;
        //        uDto.Employee = new EmployeeDTO
        //        {
        //            ID = emp.ID,
        //            FirstName = emp.FirstName,
        //            Surname = emp.Surname,
        //            PhoneNumber = emp.PhoneNumber,
        //            Email = emp.Email
        //        };
        //    }

        //    return uDto;
        //}

        //public static LocationDTO ToDto(BaseLocation location)
        //{
        //    var dto = new LocationDTO
        //    {
        //        ID = location.ID,
        //        Coordinates = new CoordinatesDTO
        //        {
        //            Latitude = location.GeoData.Latitude,
        //            Longitude = location.GeoData.Longitude,
        //            Address = location.GeoData.Address,
        //            Region = location.GeoData.Region
        //        },
        //        LocationType = location.GetType().Name
        //    };

        //    if (location is Warehouse warehouse)
        //    {
        //        dto.MaxStorageCapacity = warehouse.MaxStorageCapacity;
        //        dto.IsAutomated = warehouse.IsAutomated;
        //    }

        //    if (location is PostalOffice office)
        //    {
        //        dto.HandlesPublicDropOffs = office.HandlesPublicDropOffs;
        //        dto.IsRegionalHQ = office.IsRegionalHQ;
        //    }

        //    return dto;
        //}
        public static Delivery ToEntity(this DeliveryDTO dto) => new()
        {
            ID = dto.ID,
            Timestamp = dto.Timestamp,
            Package = dto.Package.ToEntity(),
            SenderID = dto.SenderID,
            ReceiverID = dto.ReceiverID,
            SentFromID = dto.SentFromID,
            SentToID = dto.SentToID,
            Price = dto.Price,
            IsPaid = dto.IsPaid
        };

        public static Package ToEntity(this PackageDTO dto) => new()
        {
            ID = dto.ID,
            PackageStatus = Enum.Parse<PackageStatus>(dto.PackageStatus),
            CreatedAt = dto.CreatedAt,
            Length = dto.Length,
            Width = dto.Width,
            Height = dto.Height,
            Weight = dto.Weight,
            DeclaredContent = dto.DeclaredContent.Select(c => c.ToEntity()).ToList()
        };

        public static Content ToEntity(this ContentDTO dto) => new Content
        {
            Name = dto.Name,
            Type = Enum.Parse<ContentType>(dto.Type),
            Amount = dto.Amount,
            PackageID = dto.PackageID,
            Description = dto.Description
        };

        public static User ToEntity(this UserDTO dto)
        {
            var employee = new Employee
            {
                FirstName = dto.Employee.FirstName,
                Surname = dto.Employee.Surname,
                PhoneNumber = dto.Employee.PhoneNumber,
                Email = dto.Employee.Email,
            };

            var role = new Role
            {
                ID = dto.Role.ID,
                Name = dto.Role.Name
            };

            return new User(dto.Username, "null", role, employee); // Password is null (string) because we don't store that in DTOs
        }

        public static Employee ToEntity(this EmployeeDTO dto) => new Employee
        {
            ID = dto.ID,
            FirstName = dto.FirstName,
            Surname = dto.Surname,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            WorkplaceID = dto.WorkplaceID
        };

        public static Client ToEntity(this ClientDTO dto) => new Client
        {
            ID = dto.ID,
            FirstName = dto.FirstName,
            Surname = dto.Surname,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email
        };

        public static BaseLocation ToEntity(this LocationDTO dto)
        {
            var coords = new Coordinates
            {
                Latitude = dto.Coordinates.Latitude == null 
                    ? throw new ArgumentException("Відсутня широта координат.")
                    : (double)dto.Coordinates.Latitude,
                Longitude = dto.Coordinates.Longitude == null
                    ? throw new ArgumentException("Відсутня довгота координат.")
                    : (double)dto.Coordinates.Longitude,
                Address = dto.Coordinates.Address,
                Region = dto.Coordinates.Region
            };

            return dto.LocationType switch
            {
                nameof(PostalOffice) => new PostalOffice
                {
                    ID = dto.ID,
                    GeoData = coords,
                    Staff = dto.Staff?.Select(id => new Employee { ID = id }).ToList(),
                    MaxStorageCapacity = dto.MaxStorageCapacity ?? 0,
                    IsAutomated = dto.IsAutomated ?? false,
                    HandlesPublicDropOffs = dto.HandlesPublicDropOffs ?? false,
                    IsRegionalHQ = dto.IsRegionalHQ ?? false
                },
                nameof(Warehouse) => new Warehouse
                {
                    ID = dto.ID,
                    GeoData = coords,
                    Staff = dto.Staff?.Select(id => new Employee { ID = id }).ToList(),
                    MaxStorageCapacity = dto.MaxStorageCapacity ?? 0,
                    IsAutomated = dto.IsAutomated ?? false
                },
                _ => throw new NotSupportedException($"Unknown location type: {dto.LocationType}")
            };
        }

        public static Coordinates ToEntity(this CoordinatesDTO dto) => new Coordinates
        {
            Latitude = dto.Latitude == null
                ? throw new ArgumentException("Відсутня широта координат.")
                : (double)dto.Latitude,
            Longitude = dto.Longitude == null
                ? throw new ArgumentException("Відсутня довгота координат.")
                : (double)dto.Longitude,
            Address = dto.Address,
            Region = dto.Region
        };

        public static Role ToEntity(this RoleDTO dto) => new Role
        {
            ID = dto.ID,
            Name = dto.Name,
            RolePermissions = dto.Permissions?.Select(p => new RolePermission
            {
                PermissionID = p,
                RoleID = dto.ID
            }).ToList() ?? []
        };





        public static DeliveryDTO ToDto(this Delivery delivery)
        {
            return new DeliveryDTO
            {
                ID = delivery.ID,
                Timestamp = delivery.Timestamp,
                Package = delivery.Package.ToDto(),
                SenderID = delivery.SenderID,
                ReceiverID = delivery.ReceiverID,
                SentFromID = delivery.SentFromID,
                SentToID = delivery.SentToID,
                Price = delivery.Price,
                IsPaid = delivery.IsPaid
            };
        }

        public static PackageDTO ToDto(this Package package)
        {
            return new PackageDTO
            {
                ID = package.ID,
                PackageStatus = package.PackageStatus.ToString(),
                CreatedAt = package.CreatedAt,
                Length = package.Length,
                Width = package.Width,
                Height = package.Height,
                Weight = package.Weight,
                DeclaredContent = package.DeclaredContent.Select(c => c.ToDto()).ToList(),
                Type = package.GetType().Name,
                DeliveryID = package.Delivery.ID
            };
        }

        public static ContentDTO ToDto(this Content content)
        {
            return new ContentDTO
            {
                Name = content.Name,
                Type = content.Type.ToString(),
                Amount = content.Amount,
                PackageID = content.PackageID,
                Description = content.Description
            };
        }

        public static LocationDTO ToDto(this BaseLocation location)
        {
            return new LocationDTO
            {
                ID = location.ID,
                LocationType = location.LocationType,
                Coordinates = location.GeoData.ToDto(),
                Staff = location.Staff?.Select(e => e.ID).ToList(),
                MaxStorageCapacity = (location as Warehouse)?.MaxStorageCapacity,
                IsAutomated = (location as Warehouse)?.IsAutomated,
                HandlesPublicDropOffs = (location as PostalOffice)?.HandlesPublicDropOffs,
                IsRegionalHQ = (location as PostalOffice)?.IsRegionalHQ
            };
        }

        public static CoordinatesDTO ToDto(this Coordinates coordinates)
        {
            return new CoordinatesDTO
            {
                Latitude = coordinates.Latitude,
                Longitude = coordinates.Longitude,
                Address = coordinates.Address,
                Region = coordinates.Region
            };
        }

        public static UserDTO ToDto(this User user)
        {
            return new UserDTO
            {
                ID = user.Employee.ID,
                Username = user.Username,
                Role = user.Role.ToDto(),
                Employee = user.Employee.ToDto()
            };
        }

        public static RoleDTO ToDto(this Role role)
        {
            return new RoleDTO
            {
                ID = role.ID,
                Name = role.Name,
                Permissions = role.RolePermissions?
                    .Select(rp => (uint)rp.PermissionID)
                    .Distinct()
                    .ToList() ?? []
            };
        }

        public static EmployeeDTO ToDto(this Employee employee)
        {
            return new EmployeeDTO
            {
                ID = employee.ID,
                FirstName = employee.FirstName,
                Surname = employee.Surname,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                WorkplaceID = employee.WorkplaceID,
                ManagedLocations = employee.ManagedLocations?.Select(l => l.ID).ToList() ?? new List<uint>()
            };
        }

        public static ClientDTO ToDto(this Client client)
        {
            return new ClientDTO
            {
                ID = client.ID,
                FirstName = client.FirstName,
                Surname = client.Surname,
                PhoneNumber = client.PhoneNumber,
                Email = client.Email
            };
        }
    }

}
