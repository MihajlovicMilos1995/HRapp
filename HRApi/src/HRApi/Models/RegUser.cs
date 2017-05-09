using HRApi.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HRApi.Models
{
    public class RegUser : IdentityUser
    {
        private string regUserName;
        private string regUserLastName;
        private string regUserCity;
        private string regUserCountry;
        private bool locationChange;
        private JobType regUserPartFull;
        private string workXp;
        private string regUserKeyword;

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
        public AutoGenHistory WorkHistory { get; set; }

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
    }
}

