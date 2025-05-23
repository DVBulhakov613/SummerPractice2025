using Class_Lib;
using Class_Lib.Backend.Database.Methods;
using Class_Lib.Backend.Person_related.Methods;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace OOP_CourseProject.Controls.UserObjectControls
{
    /// <summary>
    /// Interaction logic for UserDialog.xaml
    /// </summary>
    public partial class UserDialog : Window
    {
        public UserDialogViewModel ViewModel;
        private User? EditingUser;
        
        public User Result;
        private RoleMethods RoleMethods = App.AppHost.Services.GetRequiredService<RoleMethods>();
        private UserMethods UserMethods = App.AppHost.Services.GetRequiredService<UserMethods>();

        public UserDialog(User? user = null)
        {
            InitializeComponent();
            EditingUser = user;
            Loaded += UserDialog_Loaded;
        }

        private async void UserDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var allRoles = await RoleMethods.GetByCriteriaAsync(App.CurrentEmployee, u => true);
            ViewModel = new UserDialogViewModel(allRoles, EditingUser);
            DataContext = ViewModel;
        }

        private async void Ok_Click(object sender, RoutedEventArgs e)
        {
            List<string> exceptions = [];
            if(string.IsNullOrWhiteSpace(ViewModel.Username)) exceptions.Add("Назва аккаунту не може бути порожньою.");
            var existing = await UserMethods.GetByCriteriaAsync(App.CurrentEmployee, u => u.Username == ViewModel.Username);
            if (EditingUser == null && existing.Any()) exceptions.Add("Назва аккаунту вже існує.");
            if (ViewModel.IsNewUser && string.IsNullOrWhiteSpace(ViewModel.Password))
                exceptions.Add("Пароль не може бути порожнім.");
            if (ViewModel.SelectedRole == null) exceptions.Add("Необхідно обрати роль.");
            if(ViewModel.SelectedEmployee == null) exceptions.Add("Необхідно вибрати співробітника, якому буде назначено цей аккаунт.");
            
            if(exceptions.Count > 0) { 
                string message = string.Join("\n", exceptions);
                MessageBox.Show(message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Result = ViewModel.ToUser();
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

/*

        private RoleDialogViewModel ViewModel;
        private Role? EditingRole;

        public Role Result { get; private set; }
        private PermissionMethods PermissionMethods = App.AppHost.Services.GetRequiredService<PermissionMethods>();

        public RoleDialog(Role? role = null)
        {
            InitializeComponent();
            EditingRole = role;
            Loaded += RoleDialog_Loaded;
        }

        private async void RoleDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var allPermissions = await PermissionMethods.GetAllAsync(App.CurrentEmployee);
            ViewModel = new RoleDialogViewModel(allPermissions, EditingRole);
            DataContext = ViewModel;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.RoleName))
            {
                MessageBox.Show("Назва ролі не може бути порожньою.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Result = ViewModel.ToRole(EditingRole?.ID);
            DialogResult = true;
        }

*/