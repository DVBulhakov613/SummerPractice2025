using System.Windows;
using Class_Lib.Backend.Database.Repositories;
using Class_Lib.Backend.Person_related;
using Class_Lib.Backend.Services;

namespace OOP_CourseProject
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var password = PasswordBox.Password;

            using var context = App.DbContextFactory.CreateDbContext();
            var userRepository = new UserRepository(context);

            var user = await userRepository.GetByUsernameAsync(username);
            if (user == null || !PasswordHelper.VerifyPassword(password, user.PasswordHash))
            {
                MessageBox.Show("Недійсне ім'я користувача або пароль.", "Помилка авторизації", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            App.CurrentEmployee = user.Employee;

            // close the login window and open the main application
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }
}
