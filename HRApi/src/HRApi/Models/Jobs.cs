﻿using HRApi.Enums;
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
        private string jobname;
        private string jobDesc;
        private string jobCity;
        private string jobCountry;
        private JobType jobPartFull;
        private string jobKeyword;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string JobName
        {
            get
            {
                return jobname;
            }

            set
            {
                jobname = value;
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
    }
}
