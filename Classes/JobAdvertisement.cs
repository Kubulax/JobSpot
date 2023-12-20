using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace JobSpot.Classes
{
    public class JobAdvertisement
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string PositionName { get; set; }
        public string Category { get; set; }
        public string Pay { get; set; }
        public string Localization { get; set; }
        public string PositionLevel { get; set; }
        public string ContractType { get; set; }
        public string EmploymentType { get; set; }
        public string WorkType { get; set; }
        public string Duties { get; set; }
        public string Requirements { get; set; }
        public string Benefits { get; set; }
        public DateTime RecrutationEnd { get; set; }

        public JobAdvertisement(int id, string companyName, string positionName, string category, string pay, string localization, string positionLevel, string contractType, string employmentType, string workType, string duties, string requirements, string benefits, DateTime recrutationEnd)
        {
            Id = id;
            CompanyName = companyName;
            PositionName = positionName;
            Category = category;
            Pay = pay;
            Localization = localization;
            PositionLevel = positionLevel;
            ContractType = contractType;
            EmploymentType = employmentType;
            WorkType = workType;
            Duties = duties;
            Requirements = requirements;
            Benefits = benefits;
            RecrutationEnd = recrutationEnd;
        }

        public JobAdvertisement(string companyName, string positionName, string category, string pay, string localization, string positionLevel, string contractType, string employmentType, string workType, string duties, string requirements, string benefits, DateTime recrutationEnd)
        {
            CompanyName = companyName;
            PositionName = positionName;
            Category = category;
            Pay = pay;
            Localization = localization;
            PositionLevel = positionLevel;
            ContractType = contractType;
            EmploymentType = employmentType;
            WorkType = workType;
            Duties = duties;
            Requirements = requirements;
            Benefits = benefits;
            RecrutationEnd = recrutationEnd;
        }

        public JobAdvertisement()
        {

        }
    }
}
