using Class_Lib.Backend.Services;

namespace OOP_CourseProject.Controls.RoleControls
{
    public class PermissionSelectionViewModel
    {
        public Permission Permission { get; set; }
        public string Name => Permission.Name;
        public bool IsSelected { get; set; }

        public PermissionSelectionViewModel(Permission permission)
        {
            Permission = permission;
        }
    }

}
