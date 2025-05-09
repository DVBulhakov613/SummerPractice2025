using Class_Lib;
using Class_Lib.Backend.Location_related.Methods;
using Class_Lib.Backend.Package_related;
using Class_Lib.Backend.Package_related.Methods;
using Microsoft.Extensions.DependencyInjection;
using OOP_CourseProject.Controls.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OOP_CourseProject.Controls.PackageInfo
{
    /// <summary>
    /// Interaction logic for PackageInfoControl.xaml
    /// </summary>
    public partial class PackageInfoControl : UserControl
    {
        public PackageInfoControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PackageProperty =
            DependencyProperty.Register(nameof(Package), typeof(Package), typeof(PackageInfoControl), new PropertyMetadata(null, OnPackageChanged));

        public Package Package
        {
            get => (Package)GetValue(PackageProperty);
            set => SetValue(PackageProperty, value);
        }

        public IInfoProviderViewModel PackageViewModel { get; private set; }

        private static void OnPackageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PackageInfoControl control && e.NewValue is Package newPackage)
            {
                control.PackageViewModel = new PackageViewModel(newPackage);
                control.DataContext = control; // Ensure XAML bindings work properly
            }
        }
    }

}
