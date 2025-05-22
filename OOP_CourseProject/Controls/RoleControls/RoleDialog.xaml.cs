using Class_Lib.Backend.Database.Methods;
using Class_Lib.Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OOP_CourseProject.Controls.RoleControls
{
    /// <summary>
    /// Interaction logic for RoleDialog.xaml
    /// </summary>
    public partial class RoleDialog : Window
    {
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
    }
}
