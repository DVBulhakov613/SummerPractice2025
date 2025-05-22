using Class_Lib;
using Class_Lib.Backend.Database.Methods;
using Class_Lib.Backend.Serialization.DTO;
using Class_Lib.Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using OOP_CourseProject.Controls.Helpers;
using OOP_CourseProject.Controls.RoleControls;
using OOP_CourseProject.Controls.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace OOP_CourseProject.Controls
{
    /// <summary>
    /// Interaction logic for RolesControl.xaml
    /// </summary>
    public partial class RolesControl : UserControl
    {
        public GenericInfoDisplayViewModel ViewModel { get; set; }
        public RoleMethods RoleMethods { get; set; } = App.AppHost.Services.GetRequiredService<RoleMethods>();

        public async void GenerateViewModel()
        {
            ViewModel = ViewModelService.CreateViewModel(await RoleMethods.GetByCriteriaAsync(App.CurrentEmployee, r => r.ID > 0));
            DataContext = ViewModel;
        }

        public RolesControl()
        {
            InitializeComponent();
            GenerateViewModel();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new RoleDialog();
            if (dialog.ShowDialog() == true)
            {
                var newRole = dialog.Result;
                await RoleMethods.AddAsync(App.CurrentEmployee, newRole);
                RefreshButton_Click(sender, e);
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem is not Role selected) return;

            var dialog = new RoleDialog(selected);
            if (dialog.ShowDialog() == true)
            {
                var updatedRole = dialog.Result;

                selected.Name = updatedRole.Name;
                selected.RolePermissions.Clear();
                foreach (var rp in updatedRole.RolePermissions)
                    selected.RolePermissions.Add(rp);

                await RoleMethods.UpdateAsync(App.CurrentEmployee, selected);
                RefreshButton_Click(sender, e);
            }

        }

        private async void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItem is not Role selected)
            {
                MessageBox.Show("Обраний об'єкт не знайдено.", "Помилка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Видалити роль \"{selected.Name}\"?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                await RoleMethods.DeleteAsync(App.CurrentEmployee, selected);
                RefreshButton_Click(sender, e);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.UpdateItems(await RoleMethods.GetByCriteriaAsync(App.CurrentEmployee, r => r.ID > 0));
        }

        private void SerializeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SerializationHelper.SerializeSelectedItemsToFolder<Role>(RolesDataGrid, r => r.ToDto()))
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

roles are a rather complex object - they consist of permissions, which are a collection of actions that can be performed by a user with that role.
to create a new role, i would need to:
1. assign it a name
2. assign it rolepermissions

role permissions consist of several things themselves, including:
1. a role ID (which is a foreign key to the roles table)
2. a permission ID (which is a foreign key to the permissions table)

and a permission is a collection of actions, numbered in an enum - i can pull them from said enum, no big deal
a permission consists of:
1. an ID (which is a primary key)
2. a name (which is a string)


so, to create a new role, i would need to:
1. create a new role object
2. assign it a name
3. assign permissions to a list of permissions
4. from said list of permissions, construct a list of rolepermissions to connect them
5. assign the list of rolepermissions to the role object
6. save the role object to the database
and then i can use it in the application, wherever and whenever i want to

to edit a role, i would need to:
1. pull the role object from the database (already done with the datagrid, hopefully - so i can just pass it to the dialog)
2. fill out the dialog with the role object data (name, rolepermissions reconstructed into permissions?)
3. instead of making a new role object, i would just update the existing one
4. save the updated role object to the database

deletion is simple - just delete the role object from the database, and it will cascade delete all the rolepermissions associated with it and set user references to null

what a bummer..
flow:
1. open control with a datagrid of existing roles (show only the name and ID)

for adding a new role:
2. open the dialog with no information tied to it, form should include the name and a list of permissions (checkboxes? maybe the same search system as i already use elsewhere, hm)
3. when the user clicks save, i would need to create a new role object, assign it the name and permissions, and save it to the database

for editing a role:
2. open the dialog with the role object passed to it, form should include the name and a list of permissions that are already filled out and ready to edit
*/