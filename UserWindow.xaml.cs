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
    /// Logika interakcji dla klasy UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
            lbl_Nickname.Content = Title = "Profil użytkownika " + (App.Current.Properties["Session"] as User).Nickname;
        }

        private void EditProfile(object sender, RoutedEventArgs e)
        {
            new RegisterUserProfileWindow(DataContext as UserProfile).ShowDialog();

            DataContext = DataBase.ReadUserProfileByUserId((App.Current.Properties["Session"] as User).Id);
        }

        private void ShowApplications(object sender, RoutedEventArgs e)
        {
            new SearchResultWindow(DataBase.ReadApplications((App.Current.Properties["Session"] as User).Id)).Show();
            Close();
        }

        private void GoToTheMainWindow(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
