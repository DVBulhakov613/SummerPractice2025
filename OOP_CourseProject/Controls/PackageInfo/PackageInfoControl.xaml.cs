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
    public partial class PackageDetailsControl : UserControl
    {
        public PackageDetailsControl()
        {
            InitializeComponent();
            DataContext = this; // Optional: if you want the bindings in the control to resolve to the control itself
        }

        public static readonly DependencyProperty SelectedPackageIDProperty =
            DependencyProperty.Register(nameof(SelectedPackageID), typeof(string), typeof(PackageDetailsControl));

        public string SelectedPackageID
        {
            get => (string)GetValue(SelectedPackageIDProperty);
            set => SetValue(SelectedPackageIDProperty, value);
        }

        public static readonly DependencyProperty SelectedInfoViewModelProperty =
            DependencyProperty.Register(nameof(SelectedInfoViewModel), typeof(object), typeof(PackageDetailsControl));

        public object SelectedInfoViewModel
        {
            get => GetValue(SelectedInfoViewModelProperty);
            set => SetValue(SelectedInfoViewModelProperty, value);
        }
    }
}
