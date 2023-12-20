using JobSpot.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JobSpot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (!Directory.Exists(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Jobspot")))
            {
                Directory.CreateDirectory(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Jobspot"));
            }
            DataBase.CreateDB();

            RefreshUserLoggedInChanges();
        }

        private void RefreshUserLoggedInChanges()
        {
            if (App.Current.Properties["Session"] == null)
            {
                lbl_ShowNickname.Content = String.Empty;
                btn_LogOut.IsEnabled = false;
                btn_LogOut.Visibility = Visibility.Hidden;
                btn_LogInOrShowProfile.BorderBrush = Brushes.Red;
                btn_LogInOrShowProfile.IsEnabled = true;

                btn_LogInOrShowProfile.IsEnabled = true;
                lbl_adminModeInfo.Visibility = Visibility.Hidden;
                btn_AddJobAdvertisement.Visibility = Visibility.Collapsed;
                btn_AddJobAdvertisement.IsEnabled = false;
            }
            else
            {
                User? user = App.Current.Properties["Session"] as User;
                lbl_ShowNickname.Content = user.Nickname;
                btn_LogOut.IsEnabled = true;
                btn_LogOut.Visibility = Visibility.Visible;
                btn_LogInOrShowProfile.BorderBrush = Brushes.Lime;

                if (user.IsAdmin == 1)
                {
                    btn_LogInOrShowProfile.IsEnabled = false;
                    lbl_adminModeInfo.Visibility = Visibility.Visible;
                    btn_AddJobAdvertisement.Visibility = Visibility.Visible;
                    btn_AddJobAdvertisement.IsEnabled = true;
                }
                else
                {
                    btn_LogInOrShowProfile.IsEnabled = true;
                    lbl_adminModeInfo.Visibility = Visibility.Hidden;
                    btn_AddJobAdvertisement.Visibility = Visibility.Collapsed;
                    btn_AddJobAdvertisement.IsEnabled = false;
                }
            }
        }

        private void LogInOrShowProfile(object sender, RoutedEventArgs e)
        {
            if (App.Current.Properties["Session"] == null)
            {
                new LogInWindow().ShowDialog();
                RefreshUserLoggedInChanges();
            }
            else
            {
                UserWindow userWindow = new UserWindow();
                userWindow.DataContext = DataBase.ReadUserProfileByUserId((App.Current.Properties["Session"] as User).Id);
                userWindow.Show();
                Close();
            }
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            App.Current.Properties["Session"] = null;
            RefreshUserLoggedInChanges();
        }

        private void AddJobAdvertisement(object sender, RoutedEventArgs e)
        {
            AddOrEditJobAdvertisement addOrEditJobAdvertisement = new AddOrEditJobAdvertisement();
            addOrEditJobAdvertisement.ShowDialog();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            string companyName = TxtBox_CompanyName.Text;

            string positionName = TxtBox_PositionName.Text;


            string category = String.Empty;
            if (CmbBox_Category.SelectedItem != null)
            {
                category = ((ComboBoxItem)CmbBox_Category.SelectedItem).Content.ToString();
            }


            string pay = TxtBox_Pay.Text;


            string localization = TxtBox_Localization.Text;


            string positionLevel = String.Empty;
            if (CmbBox_PositionLevel.SelectedItem != null)
            {
                positionLevel = ((ComboBoxItem)CmbBox_PositionLevel.SelectedItem).Content.ToString();
            }


            string contractTpye = String.Empty;
            if (CmbBox_ContractType.SelectedItem != null)
            {
                contractTpye = ((ComboBoxItem)CmbBox_ContractType.SelectedItem).Content.ToString();
            }


            string employmentType = String.Empty;
            if (CmbBox_EmploymenType.SelectedItem != null)
            {
                employmentType = ((ComboBoxItem)CmbBox_EmploymenType.SelectedItem).Content.ToString();
            }


            string workType = String.Empty;
            if (CmbBox_WorkType.SelectedItem != null)
            {
                workType = ((ComboBoxItem)CmbBox_WorkType.SelectedItem).Content.ToString();
            }

            List<FilterParam> filters = new List<FilterParam>();

            if (!string.IsNullOrWhiteSpace(companyName))
            {
                filters.Add(new FilterParam("CompanyName", companyName));
            }
            if (!string.IsNullOrWhiteSpace(positionName))
            {
                filters.Add(new FilterParam("PositionName", positionName));
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                filters.Add(new FilterParam("Category", category));
            }
            if (!string.IsNullOrWhiteSpace(pay))
            {
                filters.Add(new FilterParam("Pay", pay));
            }
            if (!string.IsNullOrWhiteSpace(localization))
            {
                filters.Add(new FilterParam("Localization", localization));
            }
            if (!string.IsNullOrWhiteSpace(positionLevel))
            {
                filters.Add(new FilterParam("PositionLevel", positionLevel));
            }
            if (!string.IsNullOrWhiteSpace(contractTpye))
            {
                filters.Add(new FilterParam("ContractType", contractTpye));
            }
            if (!string.IsNullOrWhiteSpace(employmentType))
            {
                filters.Add(new FilterParam("EmploymentType", employmentType));
            }
            if (!string.IsNullOrWhiteSpace(workType))
            {
                filters.Add(new FilterParam("WorkType", workType));
            }

            new SearchResultWindow(filters).Show();
            Close();
        }
    }
}
