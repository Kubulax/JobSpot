using JobSpot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
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
    /// Logika interakcji dla klasy RegisterUserProfileWindow.xaml
    /// </summary>
    public partial class RegisterUserProfileWindow : Window
    {
        private bool editMode = false;
        User user = new User();
        int userProfileId;
        public RegisterUserProfileWindow(User user)
        {
            InitializeComponent();
            this.user = user;

            Title = "Zarejestruj profil użytkownika";
            btn_UniversalBtn.Content = "DODAJ";
        }
        public RegisterUserProfileWindow(UserProfile userProfile)
        {
            InitializeComponent();
            editMode = true;
            this.user = DataBase.ReadUserByName((App.Current.Properties["Session"] as User).Nickname);
            userProfileId = userProfile.Id;

            TxtBox_Name.Text = userProfile.Name;
            TxtBox_Surname.Text = userProfile.Surname;
            DatePicker_DateOfBirth.SelectedDate = userProfile.DateOfBirth;
            TxtBox_PhoneNumber.Text = userProfile.PhoneNumber;
            TxtBox_PlaceOfResidence.Text = userProfile.PlaceOfResidence;
            TxtBox_WorkExperience.Text = userProfile.WorkExperience;
            TxtBox_Education.Text = userProfile.Education;
            TxtBox_Languages.Text = userProfile.Languages;
            TxtBox_Skills.Text = userProfile.Skills;
            TxtBox_Courses.Text = userProfile.Courses;

            Title = "Edytuj profil użytkownika";
            btn_UniversalBtn.Content = "EDYTUJ";
        }

        private void RegisterUserProfile(object sender, RoutedEventArgs e)
        {
            string name = TxtBox_Name.Text;
            string surname = TxtBox_Surname.Text;
            DateTime dateOfBirth = DatePicker_DateOfBirth.DisplayDate;
            string phoneNumber = TxtBox_PhoneNumber.Text;
            string placeOfResidence = TxtBox_PlaceOfResidence.Text;
            string workExperience = TxtBox_WorkExperience.Text;
            string education = TxtBox_Education.Text;
            string languages = TxtBox_Languages.Text; ;
            string skills = TxtBox_Skills.Text;
            string courses = TxtBox_Courses.Text;

            if
            (
                string.IsNullOrWhiteSpace(name) || name.Length > 40 ||
                string.IsNullOrWhiteSpace(surname) || surname.Length > 40 ||
                dateOfBirth == DateTime.MinValue ||
                //dateOfBirth >= DateTime.Now ||
                string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(placeOfResidence)
            )
            {
                MessageBox.Show("Podano błędne dane", "Błędne dane", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(editMode)
            {
                DataBase.UpdateUserProfile(new UserProfile(userProfileId, name, surname, dateOfBirth, phoneNumber, placeOfResidence, workExperience, education, languages, skills, courses));
                MessageBox.Show("Edycja powiodła się!", "Rejestracja", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
                return;
            }

            DataBase.CreateUser(user);
            user = DataBase.ReadUserByName(user.Nickname);
            DataBase.CreateUserProfile(new UserProfile(name, surname, user.Id, dateOfBirth, phoneNumber, placeOfResidence, workExperience, education, languages, skills, courses));

            MessageBox.Show("Rejestracja powiodła się!", "Rejestracja", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
