using Class_Lib;
using Class_Lib.Backend.Person_related.Methods;
using Microsoft.Extensions.DependencyInjection;
using OOP_CourseProject.Controls.Helpers;
using OOP_CourseProject.Controls.ViewModel;
using System.Windows;
using System.Windows.Controls;
using Class_Lib.Backend.Serialization.DTO;

namespace OOP_CourseProject.Controls.UserObjectControls
{
    /// <summary>
    /// Interaction logic for UserObjectControl.xaml
    /// </summary>
    public partial class UserObjectControl : UserControl
    {
        public GenericInfoDisplayViewModel ViewModel { get; set; }
        public UserMethods UserMethods { get; set; } = App.AppHost.Services.GetRequiredService<UserMethods>();

        public async void GenerateViewModel()
        {
            ViewModel = ViewModelService.CreateViewModel(await UserMethods.GetByCriteriaAsync(App.CurrentEmployee, r => true));
            DataContext = ViewModel;
        }

        public UserObjectControl()
        {
            InitializeComponent();
            GenerateViewModel();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new UserDialog();
            if (dialog.ShowDialog() == true)
            {
                var newRole = dialog.Result;
                await UserMethods.AddAsync(App.CurrentEmployee, newRole);
                RefreshButton_Click(sender, e);
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem is not User selected) return;

            var dialog = new UserDialog(selected);
            if (dialog.ShowDialog() == true)
            {
                selected.Username = dialog.Result.Username;
                selected.PasswordHash = dialog.Result.PasswordHash;
                selected.Role = dialog.Result.Role;
                selected.Employee = dialog.Result.Employee;
                await UserMethods.UpdateAsync(App.CurrentEmployee, selected);

                RefreshButton_Click(sender, e);
            }

        }

        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem is not User selected)
            {
                MessageBox.Show("Обраний об'єкт не знайдено.", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Видалити аккаунт \"{selected.Username}\"?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                await UserMethods.DeleteAsync(App.CurrentEmployee, selected);
                RefreshButton_Click(sender, e);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateItems(await UserMethods.GetByCriteriaAsync(App.CurrentEmployee, r => r.ID > 0));
        }

        private void SerializeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SerializationHelper.SerializeSelectedItemsToFolder<User>(RolesDataGrid, r => r.ToDto()))
                    MessageBox.Show("Файли збережені успішно.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка. {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

/*

    full properties of this thing:
    
        public string Username { get; set; } // username, UNIQUE
        public string PasswordHash { get; set; } // hashed password
        public uint RoleID { get; set; } // role ID (for db purposes)
        public Role Role { get; set; } // "Administrator", "Manager", "Employee", NOT client;
        public uint? PersonID { get; set; } // Foreign key to Person table
        public Employee Employee { get; set; } // Navigation property

    should be able to create a new user, edit an existing one, delete a user, and view all users
    for editing, i should allow editing the username, password, assigned employee, and role of the user
*/