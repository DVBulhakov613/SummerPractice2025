using Class_Lib;
using Class_Lib.Backend.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OOP_CourseProject.Controls.LocationControl;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OOP_CourseProject.Controls.UserObjectControls
{
    public class UserDialogViewModel : ViewModelBase
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

        public ObservableCollection<Role> AvailableRoles { get; }
        public Role? SelectedRole { get; set; }

        public Employee? SelectedEmployee { get; set; }
        public string EmployeeName => SelectedEmployee.FullName ?? string.Empty;

        public ICommand PickEmployeeCommand { get; }

        private readonly string? _existingPasswordHash;
        public bool IsNewUser => _existingPasswordHash == null;

        public UserDialogViewModel(IEnumerable<Role> roles, User? existingUser = null)
        {
            AvailableRoles = new ObservableCollection<Role>(roles);
            PickEmployeeCommand = new RelayCommand(PickEmployee);

            if (existingUser != null)
            {
                Username = existingUser.Username;
                SelectedRole = existingUser.Role;
                SelectedEmployee = existingUser.Employee;
                _existingPasswordHash = existingUser.PasswordHash;
            }
        }

        private void PickEmployee()
        {
            var window = new EmployeeSearchWindow(
                SelectedEmployee is not null ? [SelectedEmployee] : [],
                selected =>
                {
                    if (selected.Count > 0)
                        SelectedEmployee = selected.First();
                });
            window.ShowDialog();
        }

        public User ToUser()
        {
            var hashToUse = !string.IsNullOrWhiteSpace(Password)
                ? HashPassword(Password)
                : _existingPasswordHash;

            if (hashToUse == null)
                throw new InvalidOperationException("A password must be provided for new users.");

            return new User(
                Username,
                hashToUse,
                SelectedRole ?? throw new InvalidOperationException("A role must be selected."),
                SelectedEmployee ?? throw new InvalidOperationException("An employee must be assigned.")
            );
        }

        private string HashPassword(string password)
        {
            return PasswordHelper.HashPassword(password);
        }
    }

}

/*


    public class RoleDialogViewModel
    {
        public string RoleName { get; set; } = "";
        public ObservableCollection<PermissionSelectionViewModel> Permissions { get; set; } = new();

        public RoleDialogViewModel(IEnumerable<Permission> allPermissions, Role? editingRole = null)
        {
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

*/