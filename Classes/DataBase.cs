using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;

namespace JobSpot.Classes
{
    public class DataBase
    {
        private static string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Jobspot", "jobspot.db");


        public static void CreateDB()
        {
            if (!File.Exists(dbpath))
            {
                using (var db = new SqliteConnection($"Filename={dbpath}"))
                {
                    db.Open();

                    var command = new SqliteCommand();

                    command.Connection = db;

                    command.CommandText = "CREATE TABLE IF NOT EXISTS User (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nickname TEXT, Email TEXT, Password TEXT, IsAdmin INT)";
                    command.ExecuteNonQuery();
                    command.CommandText = "CREATE TABLE IF NOT EXISTS UserProfile (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId INTEGER, Name TEXT, Surname TEXT, DateOfBirth TEXT, PhoneNumber TEXT, PlaceOfResidence TEXT, WorkExperience TEXT, Education TEXT, Languages TEXT, Skills TEXT, Courses TEXT)";
                    command.ExecuteNonQuery();
                    command.CommandText = "CREATE TABLE IF NOT EXISTS JobAdvertisement (Id INTEGER PRIMARY KEY AUTOINCREMENT, CompanyName TEXT, PositionName TEXT, Category TEXT, Pay TEXT, Localization TEXT, PositionLevel TEXT, ContractType TEXT, EmploymentType TEXT, WorkType TEXT, Duties TEXT, Requirements TEXT, Benefits TEXT, RecrutationEnd TEXT)";
                    command.ExecuteNonQuery();
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Application (Id INTEGER PRIMARY KEY AUTOINCREMENT, JobAdId INTEGER, UserId INTEGER)";
                    command.ExecuteNonQuery();

                    db.Close();
                }
                CreateUser(new User("admin", String.Empty, "admin", 1));
            }
        }


        public static void CreateUser(User user)
        {
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = @"
                INSERT INTO User
                VALUES (
                    NULL,
                    @Nickname,
                    @Email,
                    @Password,
                    @IsAdmin
                )";
                command.Parameters.AddWithValue("@Nickname", user.Nickname);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@IsAdmin", user.IsAdmin);

                command.ExecuteReader();

                db.Close();
            }
        }

        public static User ReadUserByName(string nickName)
        {
            User? user = null;

            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = "SELECT * FROM User WHERE Nickname=@Nickname";
                command.Parameters.AddWithValue("@Nickname", nickName);
                command.ExecuteNonQuery();
                SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user = new User();
                    user.Id = reader.GetInt32(0);
                    user.Nickname = reader.GetString(1);
                    user.Email = reader.GetString(2);
                    user.Password = reader.GetString(3);
                    user.IsAdmin = reader.GetInt32(4);
                }

                db.Close();
            }

            return user;
        }

        public static bool VerifyUserPassword(string nickName, string password)
        {
            User? user = null;

            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = "SELECT Id FROM User WHERE Nickname=@Nickname AND Password=@Password";
                command.Parameters.AddWithValue("@Nickname", nickName);
                command.Parameters.AddWithValue("@Password", password);
                command.ExecuteNonQuery();
                SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    db.Close();
                    return true;
                }

                db.Close();
            }

            return false;
        }


        public static void CreateUserProfile(UserProfile userProfile)
        {
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = @"
                INSERT INTO UserProfile 
                VALUES (
                    NULL,
                    @UserId,
                    @Name,
                    @Surname,
                    @DateOfBirth,
                    @PhoneNumber,
                    @PlaceOfResidence,
                    @WorkExperience,
                    @Education,
                    @Languages,
                    @Skills,
                    @Courses
                )";
                command.Parameters.AddWithValue("@UserId", userProfile.UserId);
                command.Parameters.AddWithValue("@Name", userProfile.Name);
                command.Parameters.AddWithValue("@Surname", userProfile.Surname);
                command.Parameters.AddWithValue("@DateOfBirth", userProfile.DateOfBirth.ToString());
                command.Parameters.AddWithValue("@PhoneNumber", userProfile.PhoneNumber);
                command.Parameters.AddWithValue("@PlaceOfResidence", userProfile.PlaceOfResidence);
                command.Parameters.AddWithValue("@WorkExperience", userProfile.WorkExperience);
                command.Parameters.AddWithValue("@Education", userProfile.Education);
                command.Parameters.AddWithValue("@Languages", userProfile.Languages);
                command.Parameters.AddWithValue("@Skills", userProfile.Skills);
                command.Parameters.AddWithValue("@Courses", userProfile.Courses);

                command.ExecuteReader();

                db.Close();
            }
        }

        public static UserProfile ReadUserProfileByUserId(int userId)
        {
            UserProfile userProfile = new UserProfile();

            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = "SELECT * FROM UserProfile WHERE UserId=@UserId";
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
                SqliteDataReader reader = command.ExecuteReader();
                reader.Read();
                userProfile.Id = reader.GetInt32(0);
                userProfile.UserId = reader.GetInt32(1);
                userProfile.Name = reader.GetString(2);
                userProfile.Surname = reader.GetString(3);
                userProfile.DateOfBirth = DateTime.Parse(reader.GetString(4));
                userProfile.PhoneNumber = reader.GetString(5);
                userProfile.PlaceOfResidence = reader.GetString(6);
                userProfile.WorkExperience = reader.GetString(7);
                userProfile.Education = reader.GetString(8);
                userProfile.Languages = reader.GetString(9);
                userProfile.Skills = reader.GetString(10);
                userProfile.Courses = reader.GetString(11);

                db.Close();
            }
            return userProfile;
        }

        public static void UpdateUserProfile(UserProfile userProfile)
        {
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = @"
                UPDATE UserProfile 
                SET 
                    Name = @Name, 
                    Surname = @Surname, 
                    DateOfBirth = @DateOfBirth,
                    PhoneNumber = @PhoneNumber,
                    PlaceOfResidence = @PlaceOfResidence, 
                    WorkExperience = @WorkExperience, 
                    Education = @Education,
                    Languages = @Languages,
                    Skills = @Skills,
                    Courses = @Courses 
                WHERE Id=@Id";
                command.Parameters.AddWithValue("@Name", userProfile.Name);
                command.Parameters.AddWithValue("@Surname", userProfile.Surname);
                command.Parameters.AddWithValue("@DateOfBirth", userProfile.DateOfBirth.ToString());
                command.Parameters.AddWithValue("@PhoneNumber", userProfile.PhoneNumber);
                command.Parameters.AddWithValue("@PlaceOfResidence", userProfile.PlaceOfResidence);
                command.Parameters.AddWithValue("@WorkExperience", userProfile.WorkExperience);
                command.Parameters.AddWithValue("@Education", userProfile.Education);
                command.Parameters.AddWithValue("@Languages", userProfile.Languages);
                command.Parameters.AddWithValue("@Skills", userProfile.Skills);
                command.Parameters.AddWithValue("@Courses", userProfile.Courses);
                command.Parameters.AddWithValue("@Id", userProfile.Id);

                command.ExecuteNonQuery();

                db.Close();
            }
        }


        public static void CreateJobAdvertisement(JobAdvertisement jobAdvertisement)
        {
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = @"
                INSERT INTO JobAdvertisement
                VALUES(
                    NULL,
                    @CompanyName,
                    @PositionName,
                    @Category,
                    @Pay,
                    @Localization,
                    @PositionLevel,
                    @ContractType,
                    @EmploymentType,
                    @WorkType,
                    @Duties,
                    @Requirements,
                    @Benefits,
                    @RecrutationEnd              
                )";
                command.Parameters.AddWithValue("@CompanyName", jobAdvertisement.CompanyName);
                command.Parameters.AddWithValue("@PositionName", jobAdvertisement.PositionName);
                command.Parameters.AddWithValue("@Category", jobAdvertisement.Category);
                command.Parameters.AddWithValue("@Pay", jobAdvertisement.Pay);
                command.Parameters.AddWithValue("@Localization", jobAdvertisement.Localization);
                command.Parameters.AddWithValue("@PositionLevel", jobAdvertisement.PositionLevel);
                command.Parameters.AddWithValue("@ContractType", jobAdvertisement.ContractType);
                command.Parameters.AddWithValue("@EmploymentType", jobAdvertisement.EmploymentType);
                command.Parameters.AddWithValue("@WorkType", jobAdvertisement.WorkType);
                command.Parameters.AddWithValue("@Duties", jobAdvertisement.Duties);
                command.Parameters.AddWithValue("@Requirements", jobAdvertisement.Requirements);
                command.Parameters.AddWithValue("@Benefits", jobAdvertisement.Benefits);
                command.Parameters.AddWithValue("@RecrutationEnd", jobAdvertisement.RecrutationEnd.ToString());

                command.ExecuteReader();

                db.Close();
            }
        }

        public static List<JobAdvertisement> ReadJobAdvertisements(List<FilterParam> filterParameters)
        {
            List<JobAdvertisement> jobAdvertisements = new List<JobAdvertisement>();

            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                string commandTxt = "SELECT * FROM JobAdvertisement";
                for (int i = 0; i < filterParameters.Count; i++)
                {
                    if (i == 0)
                    {
                        commandTxt += " WHERE";
                    }
                    commandTxt += " " + filterParameters[i].ColumnName + " LIKE '%" + filterParameters[i].Value + "%'";
                    if (i < filterParameters.Count - 1)
                    {
                        commandTxt += " OR ";
                    }
                }

                command.CommandText = commandTxt;
                command.ExecuteNonQuery();

                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobAdvertisement jobAdvertisement = new JobAdvertisement();

                    jobAdvertisement.Id = reader.GetInt32(0);
                    jobAdvertisement.CompanyName = reader.GetString(1);
                    jobAdvertisement.PositionName = reader.GetString(2);
                    jobAdvertisement.Category = reader.GetString(3);
                    jobAdvertisement.Pay = reader.GetString(4);
                    jobAdvertisement.Localization = reader.GetString(5);
                    jobAdvertisement.PositionLevel = reader.GetString(6);
                    jobAdvertisement.ContractType = reader.GetString(7);                 
                    jobAdvertisement.EmploymentType = reader.GetString(8);
                    jobAdvertisement.WorkType = reader.GetString(9);
                    jobAdvertisement.Duties = reader.GetString(10);
                    jobAdvertisement.Requirements = reader.GetString(11);
                    jobAdvertisement.Benefits = reader.GetString(12);
                    jobAdvertisement.RecrutationEnd = DateTime.Parse(reader.GetString(13));

                    jobAdvertisements.Add(jobAdvertisement);
                }

                db.Close();
            }
            return jobAdvertisements;
        }

        public static JobAdvertisement ReadJobAdvertisementById(int id)
        {
            JobAdvertisement jobAdvertisement = new JobAdvertisement();

            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = "SELECT * FROM JobAdvertisement WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();

                SqliteDataReader reader = command.ExecuteReader();
                reader.Read();
                jobAdvertisement.Id = reader.GetInt32(0);
                jobAdvertisement.CompanyName = reader.GetString(1);
                jobAdvertisement.PositionName = reader.GetString(2);
                jobAdvertisement.Category = reader.GetString(3);
                jobAdvertisement.Pay = reader.GetString(4);
                jobAdvertisement.Localization = reader.GetString(5);
                jobAdvertisement.PositionLevel = reader.GetString(6);
                jobAdvertisement.ContractType = reader.GetString(7);
                jobAdvertisement.EmploymentType = reader.GetString(8);
                jobAdvertisement.WorkType = reader.GetString(9);
                jobAdvertisement.Duties = reader.GetString(10);
                jobAdvertisement.Requirements = reader.GetString(11);
                jobAdvertisement.Benefits = reader.GetString(12);
                jobAdvertisement.RecrutationEnd = DateTime.Parse(reader.GetString(13));


                db.Close();
            }

            return jobAdvertisement;
        }

        public static void UpdateJobAdvertisement(JobAdvertisement jobAdvertisement)
        {
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = @"
                UPDATE JobAdvertisement
                SET 
                    CompanyName = @CompanyName, 
                    PositionName = @PositionName, 
                    Category = @Category,
                    Pay = @Pay,
                    Localization = @Localization, 
                    PositionLevel = @PositionLevel, 
                    ContractType = @ContractType,
                    EmploymentType = @EmploymentType,
                    WorkType = @WorkType,
                    Duties = @Duties,
                    Requirements = @Requirements,
                    Benefits = @Benefits,
                    RecrutationEnd = @RecrutationEnd
                WHERE Id=@Id";
                command.Parameters.AddWithValue("@CompanyName", jobAdvertisement.CompanyName);
                command.Parameters.AddWithValue("@PositionName", jobAdvertisement.PositionName);
                command.Parameters.AddWithValue("@Category", jobAdvertisement.Category);
                command.Parameters.AddWithValue("@Pay", jobAdvertisement.Pay);
                command.Parameters.AddWithValue("@Localization", jobAdvertisement.Localization);
                command.Parameters.AddWithValue("@PositionLevel", jobAdvertisement.PositionLevel);
                command.Parameters.AddWithValue("@ContractType", jobAdvertisement.ContractType);
                command.Parameters.AddWithValue("@EmploymentType", jobAdvertisement.EmploymentType);
                command.Parameters.AddWithValue("@WorkType", jobAdvertisement.WorkType);
                command.Parameters.AddWithValue("@Duties", jobAdvertisement.Duties);
                command.Parameters.AddWithValue("@Requirements", jobAdvertisement.Requirements);
                command.Parameters.AddWithValue("@Benefits", jobAdvertisement.Benefits);
                command.Parameters.AddWithValue("@RecrutationEnd", jobAdvertisement.RecrutationEnd);
                command.Parameters.AddWithValue("@Id", jobAdvertisement.Id);

                command.ExecuteNonQuery();

                db.Close();
            }
        }

        public static void DeleteJobAdvertisement(JobAdvertisement jobAdvertisement)
        {
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = "DELETE FROM JobAdvertisement WHERE Id=@Id";
                command.Parameters.AddWithValue("@Id", jobAdvertisement.Id);
                command.ExecuteNonQuery();

                command.CommandText = @"DELETE FROM Application WHERE JobAdId=@JobAdId";
                command.Parameters.AddWithValue("@JobAdId", jobAdvertisement.Id);
                command.ExecuteNonQuery();

                db.Close();
            }
        }


        public static void CreateApplication(int jobAdId, int userId)
        {
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = @"
                INSERT INTO Application 
                VALUES (
                    NULL,
                    @JobAdId,
                    @UserId
                )";
                command.Parameters.AddWithValue("@JobAdId", jobAdId);
                command.Parameters.AddWithValue("@UserId", userId);

                command.ExecuteReader();

                db.Close();
            }
        }

        public static bool CheckApplication(int jobAdId, int userId)
        {
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = @"SELECT Id FROM Application WHERE JobAdId=@JobAdId AND UserId=@UserId";
                command.Parameters.AddWithValue("@JobAdId", jobAdId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();

                SqliteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    db.Close();
                    return true;
                }
                else
                {
                    db.Close();
                    return false;
                }
            }
        }

        public static List<JobAdvertisement> ReadApplications(int userId)
        {
            List<JobAdvertisement> jobAdvertisements = new List<JobAdvertisement>();

            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                string commandTxt = "SELECT * FROM JobAdvertisement WHERE Id=(SELECT JobAdId FROM Application WHERE UserId=@UserId)";
                command.Parameters.AddWithValue("@UserId", userId);

                command.CommandText = commandTxt;
                command.ExecuteNonQuery();

                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    JobAdvertisement jobAdvertisement = new JobAdvertisement();

                    jobAdvertisement.Id = reader.GetInt32(0);
                    jobAdvertisement.CompanyName = reader.GetString(1);
                    jobAdvertisement.PositionName = reader.GetString(2);
                    jobAdvertisement.Category = reader.GetString(3);
                    jobAdvertisement.Pay = reader.GetString(4);
                    jobAdvertisement.Localization = reader.GetString(5);
                    jobAdvertisement.PositionLevel = reader.GetString(6);
                    jobAdvertisement.ContractType = reader.GetString(7);
                    jobAdvertisement.WorkType = reader.GetString(8);
                    jobAdvertisement.EmploymentType = reader.GetString(9);
                    jobAdvertisement.Duties = reader.GetString(10);
                    jobAdvertisement.Requirements = reader.GetString(11);
                    jobAdvertisement.Benefits = reader.GetString(12);
                    jobAdvertisement.RecrutationEnd = DateTime.Parse(reader.GetString(13));

                    jobAdvertisements.Add(jobAdvertisement);
                }

                db.Close();
            }
            return jobAdvertisements;
        }

        public static void DeleteApplication(int jobAdId, int userId)
        {
            using (var db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                var command = new SqliteCommand();
                command.Connection = db;

                command.CommandText = "DELETE FROM Application WHERE JobAdId=@JobAdId AND UserId=@UserId";
                command.Parameters.AddWithValue("@JobAdId", jobAdId);
                command.Parameters.AddWithValue("@UserId", userId);

                command.ExecuteNonQuery();

                db.Close();
            }
        }
    }
}
