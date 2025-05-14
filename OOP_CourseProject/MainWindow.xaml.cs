using Class_Lib;
using Class_Lib.Backend.Person_related;
using Microsoft.Extensions.DependencyInjection;
using OOP_CourseProject.Controls;
using OOP_CourseProject.Controls.SendPackageControls; // DEBUG LINE
using System.Linq.Expressions;
using System.Windows;

namespace OOP_CourseProject
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            MainContent.Content = App.AppHost.Services.GetRequiredService<HomePageControl>();
            //MainContent.Content = App.AppHost.Services.GetRequiredService<PackageConfigurations>(); // DEBUG LINE
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = App.AppHost.Services.GetRequiredService<HomePageControl>();
        }

        private void SendPackageButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = App.AppHost.Services.GetRequiredService<SendPackage>();
        }

        private void EmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = App.AppHost.Services.GetRequiredService<EmployeeControl>();
        }

        private void ReceivePackageButton_Click(object sender, RoutedEventArgs e)
        {
            // MainContent.Content = App.AppHost.Services.GetRequiredService<ReceivePackageControl>();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Test button clicked!");
        }
    }
}
