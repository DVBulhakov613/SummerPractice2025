using Class_Lib;
using System.Windows;

namespace OOP_CourseProject.Controls.LocationControl.PostalOfficeForms
{
    /// <summary>
    /// Interaction logic for PostalOfficeForm.xaml
    /// </summary>
    public partial class PostalOfficeForm : Window
    {
        public PostalOfficeViewModel ViewModel { get; private set; }

        private PostalOffice _originalModel;
        public PostalOffice PostalOfficeResult { get; private set; }

        public PostalOfficeForm()
        {
            InitializeComponent();
            ViewModel = new PostalOfficeViewModel();
            DataContext = ViewModel;
        }

        public void SetData(PostalOffice model)
        {
            _originalModel = model;
            ViewModel.LoadFromModel(model);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_originalModel != null)
                {
                    _originalModel.UpdateGeoData(new Coordinates((double)ViewModel.Longitude!, (double)ViewModel.Latitude!, ViewModel.Address, ViewModel.Region));
                    _originalModel.MaxStorageCapacity = (uint)ViewModel.MaxStorageCapacity!;
                    _originalModel.IsAutomated = (bool)ViewModel.IsAutomated!;
                    _originalModel.HandlesPublicDropOffs = (bool)ViewModel.HandlesPublicDropOffs!;
                    _originalModel.IsRegionalHQ = (bool)ViewModel.IsRegionalHQ!;
                    _originalModel.Staff = ViewModel.AssignedEmployees.ToList();

                    PostalOfficeResult = _originalModel;
                }
                else
                {
                    PostalOfficeResult = ViewModel.ToModel();
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void AddEmployees_Click(object sender, RoutedEventArgs e)
        {
            var window = new EmployeeSearchWindow(
                ViewModel.AssignedEmployees.ToList(),
                selected =>
                {
                    ViewModel.AssignedEmployees.Clear();
                    foreach (var employee in selected)
                    {
                        ViewModel.AssignedEmployees.Add(employee);
                    }
                });

            window.Owner = this;
            window.ShowDialog();
        }
    }
}