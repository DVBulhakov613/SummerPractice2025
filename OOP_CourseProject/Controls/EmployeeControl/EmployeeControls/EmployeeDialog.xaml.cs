using Class_Lib;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Class_Lib.Backend.Services;
using Microsoft.Extensions.DependencyInjection;
using Class_Lib.Backend.Database.Repositories;

namespace OOP_CourseProject.Controls.EmployeeControl
{
    public partial class EmployeeDialog : Window, INotifyPropertyChanged
    {
        private readonly Employee _originalEmployee;
        public Employee Result { get; private set; }

        private BaseLocation _selectedWorkplace;
        public BaseLocation SelectedWorkplace
        {
            get => _selectedWorkplace;
            set
            {
                if (_selectedWorkplace != value)
                {
                    _selectedWorkplace = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(WorkplaceDisplay));
                }
            }
        }

        public string WorkplaceDisplay => SelectedWorkplace != null
            ? $"{SelectedWorkplace.GeoData.Region}, {SelectedWorkplace.GeoData.Address} (ID: {SelectedWorkplace.ID})"
            : string.Empty;

        public EmployeeDialog(Employee existing = null)
        {
            InitializeComponent();
            DataContext = this;

            if (existing != null)
            {
                _originalEmployee = existing;

                FirstNameBox.Text = existing.FirstName;
                SurnameBox.Text = existing.Surname;
                PhoneBox.Text = existing.PhoneNumber;
                EmailBox.Text = existing.Email;
                SelectedWorkplace = existing.Workplace!;
            }
        }


        private void SelectWorkplace_Click(object sender, RoutedEventArgs e)
        {
            var window = new SendPackageControls.LocationSearchWindow(location =>
            {
                SelectedWorkplace = location;
            });
            window.ShowDialog();
        }

        private async void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_originalEmployee != null)
                {
                    _originalEmployee.FirstName = FirstNameBox.Text;
                    _originalEmployee.Surname = SurnameBox.Text;
                    _originalEmployee.PhoneNumber = PhoneBox.Text;
                    _originalEmployee.Email = EmailBox.Text;
                    _originalEmployee.Workplace = SelectedWorkplace;
                    Result = _originalEmployee;
                    if(Result.User == null) // if the user is not created yet, assign a new user account
                    {
                        Result.User = new User(
                            $"{Result.FirstName.ToLower()}.{Result.Surname.ToLower()}",
                            PasswordHelper.HashPassword($"{Result.FirstName.ToLower()}.{Result.Surname.ToLower()}"),
                            await App.AppHost.Services.GetRequiredService<RoleRepository>().GetRoleByNameAsync("Працівник"),
                            Result);
                    }
                }
                else
                {
                    Result = new Employee(
                    FirstNameBox.Text,
                    SurnameBox.Text,
                    PhoneBox.Text,
                    EmailBox.Text,
                    "Працівник",
                    SelectedWorkplace);
                    Result.User = new User( // if the user is not created yet, assign a new user account
                        $"{Result.FirstName.ToLower()}.{Result.Surname.ToLower()}",
                        PasswordHelper.HashPassword($"{Result.FirstName.ToLower()}.{Result.Surname.ToLower()}"),
                        await App.AppHost.Services.GetRequiredService<RoleRepository>().GetRoleByNameAsync("Працівник"),
                        Result);
                }
                
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}