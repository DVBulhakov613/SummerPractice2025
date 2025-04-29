using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace OOP_CourseProject.Controls.PackageInfo
{
    public partial class PackageInfoControl : UserControl
    {
        public PackageInfoControl()
        {
            InitializeComponent();
        }

        // placeholders for when i get a proper info management tab

        private void ArrivalPoint_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Arrival point clicked!");
        }

        private void SenderPhone_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sender phone clicked!");
        }

        private void ReceiverPhone_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Receiver phone clicked!");
        }

        private void DeparturePoint_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Departure point clicked!");
        }
    }
}
