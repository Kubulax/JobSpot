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
    /// Logika interakcji dla klasy JobAdWindow.xaml
    /// </summary>
    public partial class JobAdWindow : Window
    {
        private List<FilterParam> filterParams = new List<FilterParam>();
        private bool gotHereFromApplicationsPage;
        public JobAdWindow(int jobAdId, List<FilterParam> filterParams, bool gotHereFromApplicationsPage)
        {
            InitializeComponent();

            if (App.Current.Properties["Session"] != null)
            {
                int userId = (App.Current.Properties["Session"] as User).Id;

                if (!DataBase.CheckApplication(jobAdId, userId))
                {
                    Btn_Apply.Content = "APLIKUJ";
                }
                else
                {
                    Btn_Apply.Content = "PRZESTAŃ APLIKOWAĆ";
                }

                Btn_Apply.Visibility = Visibility.Visible;
                Btn_Apply.IsEnabled = true;


                if ((App.Current.Properties["Session"] as User).IsAdmin == 1)
                {
                    Btn_EditJobAd.Visibility = Visibility.Visible;
                    Btn_EditJobAd.IsEnabled = true;

                    Btn_Apply.Visibility = Visibility.Collapsed;
                    Btn_Apply.IsEnabled = false;
                }
                else
                {
                    Btn_EditJobAd.Visibility = Visibility.Collapsed;
                    Btn_EditJobAd.IsEnabled = false;
                }
            }
            else
            {
                Btn_Apply.Visibility = Visibility.Collapsed;
                Btn_Apply.IsEnabled = false;
            }

            this.filterParams = filterParams;
            this.gotHereFromApplicationsPage = gotHereFromApplicationsPage;

            Title = "Strona oferty pracy";
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            if(gotHereFromApplicationsPage)
            {
                new SearchResultWindow(DataBase.ReadApplications((App.Current.Properties["Session"] as User).Id)).Show();
                Close();
                return;
            }

            new SearchResultWindow(filterParams).Show();
            Close();
        }

        private void Apply(object sender, RoutedEventArgs e)
        {
            int jobAdId = (DataContext as JobAdvertisement).Id;
            int userId = (App.Current.Properties["Session"] as User).Id;


            if (DataBase.CheckApplication(jobAdId, userId))
            {
                DataBase.DeleteApplication(jobAdId, userId);
                Btn_Apply.Content = "APLIKUJ";
                return;
            }
            DataBase.CreateApplication(jobAdId, userId);
            Btn_Apply.Content = "PRZESTAŃ APLIKOWAĆ";
        }

        private void EditJobAd(object sender, RoutedEventArgs e)
        {
            AddOrEditJobAdvertisement addOrEditJobAdvertisement = new AddOrEditJobAdvertisement(DataContext as JobAdvertisement);
            addOrEditJobAdvertisement.ShowDialog();

            DataContext = DataBase.ReadJobAdvertisementById((DataContext as JobAdvertisement).Id);
        }
    }
}
