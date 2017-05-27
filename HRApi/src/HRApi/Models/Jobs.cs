using HRApi.Enums;
using System;
using System.Collections.Generic;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int JobId { get; set; }

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

        [RegularExpression("^[0-9]*$", ErrorMessage = "Salary must be numeric")]
        public string JobSalary
        {
            get
            {
                return JobSalary;
            }

            set
            {
                JobSalary = value;
            }
        }
    }
}
