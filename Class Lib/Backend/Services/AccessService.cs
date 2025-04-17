using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Services
{
    public static class AccessService
    {
        private static readonly List<Type> AllRoles = new() { typeof(Employee) };
        private static readonly List<Type> AdminAndManagerRoles = new() { typeof(Manager) };
        private static readonly List<Type> AdminOnlyRoles = new() { typeof(Administrator) };

        private static readonly Dictionary<string, Dictionary<string, List<Type>>> Permissions = new()
        {
            // scope permissions
            { "Scope", new Dictionary<string, List<Type>>
                {
                    { "LocalPermissions", AllRoles },
                    { "GlobalPermissions", AdminOnlyRoles }
                }
            },

            // package permissions
            { "Package", new Dictionary<string, List<Type>>
                {
                    { "ReadPackage", AllRoles },
                    { "CreatePackage", AllRoles },
                    { "UpdatePackage", AdminAndManagerRoles },
                    { "DeletePackage", AdminAndManagerRoles }
                }
            },

            // package event permissions
            { "Event", new Dictionary<string, List<Type>>
                {
                    { "ReadEvent", AllRoles },
                    { "CreateEvent", AllRoles },
                    { "UpdateEvent", AdminAndManagerRoles },
                    { "DeleteEvent", AdminOnlyRoles }
                }
            },

            // content permissions
            { "Content", new Dictionary<string, List<Type>>
                {
                    { "ReadContent", AllRoles },
                    { "CreateContent", AllRoles },
                    { "UpdateContent", AllRoles },
                    { "DeleteContent", AdminAndManagerRoles }
                }
            },

            // person permissions
            { "Person", new Dictionary<string, List<Type>>
                {
                    { "ReadPerson", AllRoles },
                    { "CreatePerson", AdminAndManagerRoles },
                    { "UpdatePerson", AdminAndManagerRoles },
                    { "DeletePerson", AdminAndManagerRoles }
                }
            },

            // location permissions
            { "Location", new Dictionary<string, List<Type>>
                {
                    { "ReadLocation", AllRoles },
                    { "CreateLocation", AdminAndManagerRoles },
                    { "UpdateLocation", AdminAndManagerRoles },
                    { "DeleteLocation", AdminAndManagerRoles }
                }
            },

            // report permissions
            {
                "Report", new Dictionary<string, List<Type>>
                {
                    { "ReadReport", AllRoles },
                    { "CreateReport", AdminAndManagerRoles },
                    { "UpdateReport", AdminAndManagerRoles },
                    { "DeleteReport", AdminAndManagerRoles }
                }
            },

            // delivery vehicle permissions
            { "DeliveryVehicle", new Dictionary<string, List<Type>>
                {
                    { "CreateDeliveryVehicle", AdminAndManagerRoles },
                    { "UpdateDeliveryVehicle", AdminAndManagerRoles },
                    { "DeleteDeliveryVehicle", AdminAndManagerRoles }
                }
            },

            // database-related and constant-related permissions
            { "Database", new Dictionary<string, List<Type>>
                {
                    { "AddContentType", AdminOnlyRoles },
                    { "UpdateContentType", AdminOnlyRoles },
                    { "DeleteContentType", AdminOnlyRoles },

                    { "AddPackageStatus", AdminOnlyRoles },
                    { "UpdatePackageStatus", AdminOnlyRoles },
                    { "DeletePackageStatus", AdminOnlyRoles },

                    { "AddPackageType", AdminOnlyRoles },
                    { "UpdatePackageType", AdminOnlyRoles },
                    { "DeletePackageType", AdminOnlyRoles },

                    { "AddCountry", AdminOnlyRoles },
                    { "UpdateCountry", AdminOnlyRoles },
                    { "DeleteCountry", AdminOnlyRoles }
                }
            }
        };

        public static bool CanPerformAction(Type role, string action)
        {
            foreach (var category in Permissions.Values)
            {
                if (category.TryGetValue(action, out var allowedRoles))
                {
                    foreach (var allowedRole in allowedRoles)
                    {
                        // This allows roles that inherit from allowedRole
                        if (allowedRole.IsAssignableFrom(role))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
