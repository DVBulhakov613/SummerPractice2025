using OOP_CourseProject.Controls.ViewModel.ViewModels;
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

namespace OOP_CourseProject.Controls.PackageInfo
{
    /// <summary>
    /// Interaction logic for ContentItemListWindow.xaml
    /// </summary>
    public partial class ContentItemListWindow : Window
    {
        public ContentItemListWindow(ContentInfoViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
