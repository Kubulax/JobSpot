using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSpot.Classes
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Surname { get; set; }
        public int UserId { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string PlaceOfResidence { get; set; }
        public string WorkExperience { get; set; }
        public string Education { get; set; }
        public string Languages { get; set; }
        public string Skills { get; set; }
        public string Courses { get; set; }

        public UserProfile(int id, string name, string surname, DateTime dateOfBirth, string phoneNumber, string placeOfResidence, string workExperience, string education, string languages, string skills, string courses)
        {
            Id = id;
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            PlaceOfResidence = placeOfResidence;
            WorkExperience = workExperience;
            Education = education;
            Languages = languages;
            Skills = skills;
            Courses = courses;
        }

        public UserProfile(string name, string surname, int userId, DateTime dateOfBirth, string phoneNumber, string placeOfResidence, string workExperience, string education, string languages, string skills, string courses)
        {
            Name = name;
            Surname = surname;
            UserId = userId;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            PlaceOfResidence = placeOfResidence;
            WorkExperience = workExperience;
            Education = education;
            Languages = languages;
            Skills = skills;
            Courses = courses;
        }

        public UserProfile()
        {

        }
    }
}
