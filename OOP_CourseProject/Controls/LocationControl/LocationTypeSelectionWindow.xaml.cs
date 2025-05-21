using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for LocationTypeSelectionWindow.xaml
    /// </summary>
    public partial class LocationTypeSelectionWindow : Window
    {
        public string SelectedLocationType { get; private set; }

        public LocationTypeSelectionWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (TypeComboBox.SelectedItem is ComboBoxItem item && item.Tag is string tag)
            {
                SelectedLocationType = tag;
                DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }

}
