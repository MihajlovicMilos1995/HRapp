using HRApi.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRApi.Models
{
    public class RegUser : IdentityUser
    {
        private string regUserName;
        private string regUserLastName;
        private Sex regUserSex;
        private DateTime regUserDoB;
        private string regUserCity;
        private string regUserCountry;
        private bool locationChange;
        private JobType regUserPartFull;
        private string workXp;
        public Status StatusOfUser;
        private string regUserKeyword;
        private string regUserAdditionalInfo;
        [NotMapped]
        public string Password { get; set; }

        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string RegUserName
        {
            get
            {
                return regUserName;
            }

            set
            {
                regUserName = value;
            }
        }
               
        public string RegUserLastName
        {
            get
            {
                return regUserLastName;
            }

            set
            {
                regUserLastName = value;
            }
        }

        public string RegUserCity
        {
            get
            {
                return regUserCity;
            }

            set
            {
                regUserCity = value;
            }
        }

        public string RegUserCountry
        {
            get
            {
                return regUserCountry;
            }

            set
            {
                regUserCountry = value;
            }
        }

        public bool LocationChange
        {
            get
            {
                return locationChange;
            }

            set
            {
                locationChange = value;
            }
        }

        public JobType RegUserPartFull
        {
            get
            {
                return regUserPartFull;
            }

            set
            {
                regUserPartFull = value;
            }
        }

        public string WorkXp
        {
            get
            {
                return workXp;
            }

            set
            {
                workXp = value;
            }
        }

        public string RegUserKeyword
        {
            get
            {
                return regUserKeyword;
            }

            set
            {
                regUserKeyword = value;
            }
        }
        public ICollection<AutoGenHistory> WorkHistory { get; set; }

        public Sex RegUserSex
        {
            get
            {
                return regUserSex;
            }

            set
            {
                regUserSex = value;
            }
        }

        public DateTime RegUserDoB
        {
            get
            {
                return regUserDoB;
            }

            set
            {
                regUserDoB = value;
            }
        }

        public string RegUserAdditionalInfo
        {
            get
            {
                return regUserAdditionalInfo;
            }

            set
            {
                regUserAdditionalInfo = value;
            }
        }
    }
}

