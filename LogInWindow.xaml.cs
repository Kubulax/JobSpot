using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using JobSpot.Classes;
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
    /// Logika interakcji dla klasy LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        private void LogIn(object sender, RoutedEventArgs e)
        {
            string nickName = TxtBox_Login.Text;
            string password = PasswordBox_Password.Password;

            if (DataBase.VerifyUserPassword(nickName, password))
            {
                App.Current.Properties["Session"] = DataBase.ReadUserByName(nickName);
                Close();
            }
            else
            {
                MessageBox.Show("Błędne dane logowania.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoToRegisterWindow(object sender, RoutedEventArgs e)
        {
            new RegisterWindow().Show();
            Close();
        }
    }
}
