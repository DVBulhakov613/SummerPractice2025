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

namespace OOP_CourseProject.Controls.LocationControl
{
    /// <summary>
    /// Interaction logic for LocationDialog.xaml
    /// </summary>
    public partial class LocationDialog : Window
    {
        public LocationDialog()
        {
            InitializeComponent();
        }

        private void TypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is LocationDialogViewModel vm &&
                TypeSelector.SelectedItem is ComboBoxItem item)
            {
                vm.SwitchTo(item.Tag.ToString());
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is LocationDialogViewModel vm)
            {
                vm.Confirm();
                DialogResult = true;
                Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

    }
}
