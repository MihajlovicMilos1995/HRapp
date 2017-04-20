using HRApi.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HRApi.Models
{
    public class RegUser
    {
        private string regUserName;
        private string regUserLastName;
        private string regUserCity;
        private string regUserCountry;
        private YesNo locationChange;
        private PartFull regUserPartFull;
        private string workXp;
        private string regUserKeyword;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int RegUserId { get; set; }

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

        public YesNo LocationChange
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

        public PartFull RegUserPartFull
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
        public AutoGenWorkHistory WorkHistory { get; set; }
    }
}
}
