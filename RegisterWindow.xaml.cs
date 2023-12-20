using JobSpot.Classes;
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

namespace JobSpot
{
    /// <summary>
    /// Logika interakcji dla klasy RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            string nickname = TxtBox_Login.Text;
            string email = TxtBox_Email.Text;
            string password = PasswordBox_Password.Password;
            string confirmPassword = PasswordBox_Password.Password;

            if 
            (
                string.IsNullOrWhiteSpace(nickname) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword)
            )
            {
                MessageBox.Show("Proszę wprowadzić dane do rejestracji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(DataBase.ReadUserByName(nickname) != null)
            {
                MessageBox.Show("Podana nazwa użytkownika jest już zajęta.", "Nazwa użytkownika", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if(!ChkBox_Agreement.IsChecked.Value)
            {
                MessageBox.Show("Aby się zarejestrować należy zaakceptować warunki umowy użytkownika.", "Umowa użytkownika", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if(password != confirmPassword)
            {
                MessageBox.Show("Hasła nie są zgodne.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            new RegisterUserProfileWindow(new User(nickname, email, password, 0)).Show();
            Close();
        }
    }
}
