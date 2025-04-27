using System.Windows;
using Class_Lib.Backend.Services;

namespace OOP_CourseProject
{
    public partial class PasswordHashingWindow : Window
    {
        public PasswordHashingWindow()
        {
            InitializeComponent();
        }

        private void HashPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var password = PasswordBox.Password;
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter a password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var hashedPassword = PasswordHelper.HashPassword(password);
            HashedPasswordTextBox.Text = hashedPassword;
        }
    }
}