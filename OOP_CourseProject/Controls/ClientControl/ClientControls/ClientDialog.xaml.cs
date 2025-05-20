using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Class_Lib;
using Class_Lib.Backend.Person_related;

namespace OOP_CourseProject.Controls.ClientControl.ClientControls
{
    /// <summary>
    /// Interaction logic for ClientDialog.xaml
    /// </summary>
    public partial class ClientDialog : Window
    {
        private readonly Client _originalClient;
        public Client Result { get; private set; }
        

        public ClientDialog(Client existing = null)
        {
            InitializeComponent();
            if (existing != null)
            {
                _originalClient = existing;

                FirstNameBox.Text = existing.FirstName;
                SurnameBox.Text = existing.Surname;
                PhoneBox.Text = existing.PhoneNumber;
                EmailBox.Text = existing.Email;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_originalClient != null)
                {
                    _originalClient.FirstName = FirstNameBox.Text;
                    _originalClient.Surname = SurnameBox.Text;
                    _originalClient.PhoneNumber = PhoneBox.Text;
                    _originalClient.Email = EmailBox.Text;
                    Result = _originalClient;
                    DialogResult = true;
                    return;
                }
                Result = new Client(
                    FirstNameBox.Text,
                    SurnameBox.Text,
                    PhoneBox.Text,
                    EmailBox.Text
                );
                DialogResult = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}