using Class_Lib;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOP_CourseProject.Controls.SendPackageControls
{
    /// <summary>
    /// Interaction logic for PackageConfigurations.xaml
    /// </summary>
    public partial class PackageConfigurations : UserControl
    {
        public ObservableCollection<PackageType> PackageTypes
        {
            get => (ObservableCollection<PackageType>)GetValue(PackageTypesProperty);
            set => SetValue(PackageTypesProperty, value);
        }

        public static readonly DependencyProperty PackageTypesProperty =
            DependencyProperty.Register(nameof(PackageTypes), typeof(ObservableCollection<PackageType>), typeof(PackageConfigurations));

        public PackageType SelectedPackageType
        {
            get => (PackageType)GetValue(SelectedPackageTypeProperty);
            set => SetValue(SelectedPackageTypeProperty, value);
        }

        public static readonly DependencyProperty SelectedPackageTypeProperty =
            DependencyProperty.Register(nameof(SelectedPackageType), typeof(PackageType), typeof(PackageConfigurations));

        public PackageConfigurations()
        {
            InitializeComponent();

            LoadEnums();
        }

        public void LoadEnums()
        {
            if (PackageTypes == null)
                PackageTypes = new ObservableCollection<PackageType>();
            else
                PackageTypes.Clear();

            foreach (PackageType value in Enum.GetValues(typeof(PackageType)))
                PackageTypes.Add(value);
        }

    }
}
