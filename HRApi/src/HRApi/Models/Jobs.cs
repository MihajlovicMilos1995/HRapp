using HRApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HRApi.Models
{
    public class Job
    {
        private string jobName;
        private string jobDesc;
        private string jobCity;
        private string jobCountry;
        private string jobCategories;
        private string jobSalary;
        private string jobReqXp;
        private JobType jobPartFull;
        private string jobKeyword;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int JobId { get; set; }

        [DisplayName("Name")]
        public string JobName
        {
            get
            {
                return jobName;
            }

            set
            {
                jobName = value;
            }
        }

        [DisplayName("Description")]
        public string JobDesc
        {
            get
            {
                return jobDesc;
            }

            set
            {
                jobDesc = value;
            }
        }

        [DisplayName("City")]
        public string JobCity
        {
            get
            {
                return jobCity;
            }

            set
            {
                jobCity = value;
            }
        }

        [DisplayName("Country")]
        public string JobCountry
        {
            get
            {
                return jobCountry;
            }

            set
            {
                jobCountry = value;
            }
        }

        [DisplayName("Full/Part time")]
        public JobType JobPartFull
        {
            get
            {
                return jobPartFull;
            }

            set
            {
                jobPartFull = value;
            }
        }

        [DisplayName("Keyword")]
        public string JobKeyword
        {
            get
            {
                return jobKeyword;
            }

            set
            {
                jobKeyword = value;
            }
        }

        [DisplayName("Categorie")]
        public string JobCategories
        {
            get
            {
                return jobCategories;
            }

            set
            {
                jobCategories = value;
            }
        }

        [DisplayName("Reqired experience")]
        public string JobReqXp
        {
            get
            {
                return jobReqXp;
            }

            set
            {
                jobReqXp = value;
            }
        }

        [DisplayName("Salary")]

        [RegularExpression("^[0-9]*$", ErrorMessage = "Salary must be numeric")]
        public string JobSalary
        {
            get
            {
                return jobSalary;
            }

            set
            {
                jobSalary = value;
            }
        }
    }
}
