using JobSpot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace JobSpot
{
    /// <summary>
    /// Logika interakcji dla klasy AddOrEditJobAdvertisement.xaml
    /// </summary>
    public partial class AddOrEditJobAdvertisement : Window
    {
        private bool editMode = false;
        int jobAdvertisementId;
        public AddOrEditJobAdvertisement()
        {
            InitializeComponent();

            btn_AddOrEditJobAdvert.Content = "DODAJ";
        }
        public AddOrEditJobAdvertisement(JobAdvertisement jobAdvertisement)
        {
            InitializeComponent();

            editMode = true;
            jobAdvertisementId = jobAdvertisement.Id;

            TxtBox_CompanyName.Text = jobAdvertisement.CompanyName;
            TxtBox_PositionName.Text = jobAdvertisement.PositionName;
            CmbBox_Category.Text = jobAdvertisement.Category;
            TxtBox_Pay.Text = jobAdvertisement.Pay;
            TxtBox_Localization.Text = jobAdvertisement.Localization;
            CmbBox_PositionLevel.Text = jobAdvertisement.PositionLevel;
            CmbBox_ContractType.Text = jobAdvertisement.ContractType;
            CmbBox_EmploymenType.Text = jobAdvertisement.EmploymentType;
            CmbBox_WorkType.Text = jobAdvertisement.WorkType;
            TxtBox_Duties.Text = jobAdvertisement.Duties;
            TxtBox_Requirements.Text = jobAdvertisement.Requirements;
            TxtBox_Benefits.Text = jobAdvertisement.Benefits;
            DatePicker_RecrutationEnd.SelectedDate = jobAdvertisement.RecrutationEnd;

            btn_AddOrEditJobAdvert.Content = "EDYTUJ";
        }

        private void AddOrEditJobAdvert(object sender, RoutedEventArgs e)
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


            string contractType = String.Empty;
            if (CmbBox_ContractType.SelectedItem != null)
            {
                contractType = ((ComboBoxItem)CmbBox_ContractType.SelectedItem).Content.ToString();
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

            string duties = TxtBox_Duties.Text;
            string requirements = TxtBox_Requirements.Text;
            string benefits = TxtBox_Benefits.Text;
            DateTime recrutationEnd = DatePicker_RecrutationEnd.DisplayDate; 

            if
            (
                string.IsNullOrWhiteSpace(companyName) ||
                string.IsNullOrWhiteSpace(positionName) ||
                string.IsNullOrWhiteSpace(category) ||
                string.IsNullOrWhiteSpace(pay) ||
                string.IsNullOrWhiteSpace(contractType) ||
                string.IsNullOrWhiteSpace(employmentType) ||
                string.IsNullOrWhiteSpace(workType)
            )
            {
                MessageBox.Show("Proszę wypełnić wymagane pola.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
/*
            if(recrutationEnd < DateTime.Now)
            {
                MessageBox.Show("Proszę wprowadzić poprawną datę końca rekrutacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
*/
            if(editMode)
            {
                JobAdvertisement jobAdvertisement = new JobAdvertisement(jobAdvertisementId, companyName, positionName, category, pay, localization, positionLevel, contractType, employmentType, workType, duties, requirements, benefits, recrutationEnd);
                DataBase.UpdateJobAdvertisement(jobAdvertisement);
                MessageBox.Show("Edycja powiodła się!", "Dodawanie ogłoszenia", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
                return;
            }

            DataBase.CreateJobAdvertisement(new JobAdvertisement(companyName, positionName, category, pay, localization, positionLevel, contractType, employmentType, workType, duties, requirements, benefits, recrutationEnd));
            MessageBox.Show("Pomyślnie dodano ogłoszenie!", "Dodawanie ogłoszenia", MessageBoxButton.OK, MessageBoxImage.Information);
            new AddOrEditJobAdvertisement().Show();
            Close();
        }
    }
}
