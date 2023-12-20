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
    /// Logika interakcji dla klasy SearchResultWindow.xaml
    /// </summary>
    public partial class SearchResultWindow : Window
    {
        List<FilterParam> filterParams = new List<FilterParam>();
        private bool gotHereFromUserWindow = false;
        public SearchResultWindow(List<FilterParam> filterParams)
        {
            InitializeComponent();
            ListView_JobAdvertisements.ItemsSource = DataBase.ReadJobAdvertisements(filterParams);

            this.filterParams = filterParams;

            Title = "Wyniki wyszukiwania";
        }
        public SearchResultWindow(List<JobAdvertisement> jobAdvertisementss)
        {
            InitializeComponent();
            ListView_JobAdvertisements.ItemsSource = jobAdvertisementss;

            gotHereFromUserWindow = true;

            Title = "Aplikacje użytkownika " + (App.Current.Properties["Session"] as User).Nickname;
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            if(gotHereFromUserWindow)
            {
                UserWindow userWindow = new UserWindow();
                userWindow.DataContext = DataBase.ReadUserProfileByUserId((App.Current.Properties["Session"] as User).Id);
                userWindow.Show();
                Close();
                return;
            }

            var MainWindow = new MainWindow();
            MainWindow.Show();
            Close();
        }

        private void GoToTheJobAdWindow(object sender, MouseButtonEventArgs e)
        {
            JobAdvertisement jobAdvertisement = ListView_JobAdvertisements.SelectedItem as JobAdvertisement;

            if(jobAdvertisement != null)
            {
                JobAdWindow jobAdWindow = new JobAdWindow(jobAdvertisement.Id, filterParams, false);
                if (gotHereFromUserWindow)
                {
                    jobAdWindow = new JobAdWindow(jobAdvertisement.Id, filterParams, true);
                }

                jobAdWindow.DataContext = jobAdvertisement;
                jobAdWindow.Show();
                Close();
            }
        }
    }
}
