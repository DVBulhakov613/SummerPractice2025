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
                    _originalModel.UpdateGeoData(new Coordinates(ViewModel.Longitude, ViewModel.Latitude, ViewModel.Address, ViewModel.Region));
                    _originalModel.MaxStorageCapacity = ViewModel.MaxStorageCapacity;
                    _originalModel.IsAutomated = ViewModel.IsAutomated;
                    _originalModel.HandlesPublicDropOffs = ViewModel.HandlesPublicDropOffs;
                    _originalModel.IsRegionalHQ = ViewModel.IsRegionalHQ;

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
    }


}
