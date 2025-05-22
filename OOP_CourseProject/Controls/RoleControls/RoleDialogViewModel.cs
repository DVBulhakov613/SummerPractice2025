using Class_Lib.Backend.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_CourseProject.Controls.RoleControls
{
    public class RoleDialogViewModel
    {
        public string RoleName { get; set; } = "";
        public ObservableCollection<PermissionSelectionViewModel> Permissions { get; set; } = new();

        public RoleDialogViewModel(IEnumerable<Permission> allPermissions, Role? editingRole = null)
        {
            // Gather selected permission IDs from the RolePermissions junction
            var selectedPermissionIds = editingRole?.RolePermissions?.Select(rp => rp.PermissionID).ToHashSet() ?? new();

            foreach (var perm in allPermissions)
            {
                Permissions.Add(new PermissionSelectionViewModel(perm)
                {
                    IsSelected = selectedPermissionIds.Contains(perm.ID)
                });
            }

            if (editingRole != null)
            {
                RoleName = editingRole.Name;
            }
        }

        public Role ToRole(uint? id = null)
        {
            return new Role
            {
                ID = (uint)(id ?? 0),
                Name = RoleName,
                RolePermissions = Permissions
                    .Where(p => p.IsSelected)
                    .Select(p => new RolePermission
                    {
                        PermissionID = p.Permission.ID
                    })
                    .ToList()
            };
        }
    }
}
