using Class_Lib;
using Class_Lib.Backend.Person_related;
using OOP_CourseProject.Controls;
using System.Linq.Expressions;
using System.Windows;

namespace OOP_CourseProject
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainContent.Content = new HomePageControl();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HomePageControl();
        }

        private void SendPackageButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SendPackageControl();
        }
    }
}
