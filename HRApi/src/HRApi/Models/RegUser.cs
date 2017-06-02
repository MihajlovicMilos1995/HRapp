using HRApi.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        public UserStatus statusOfUser;
        private string regUserKeyword;
        private string regUserAdditionalInfo;
        [NotMapped]
        public string Password { get; set; }

       // [Key]
       // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       // public int Id { get; set; }

        [DisplayName("First Name")]
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
        [DisplayName("Status")]
        public UserStatus StatusOfUser
        {
            get
            {
                return statusOfUser;
            }

            set
            {
                statusOfUser = value;
            }
        }

        [DisplayName("Last Name")]
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

        [DisplayName("City")]
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

        [DisplayName("Country")]
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

        [DisplayName("Location change")]
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

        [DisplayName("Part/Full time")]
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

        [DisplayName("Experience")]
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

        [DisplayName("Keywords")]
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

        [DisplayName("Sex")]
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

        [DisplayName("Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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

        [DisplayName("Additional information")]
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

