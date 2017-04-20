using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HRApi.Models
{
    public class Company
    {
        private string companyName;
        private string companyDesc;
        private string companyCity;
        private string companyCountry;
        private string companyPhone;
        private string companyEmail;
        private string companyWebSite;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int CompanyId { get; set; }

        public string CompanyName
        {
            get
            {
                return companyName;
            }

            set
            {
                companyName = value;
            }
        }

        public string CompanyDesc
        {
            get
            {
                return companyDesc;
            }

            set
            {
                companyDesc = value;
            }
        }

        public string CompanyCity
        {
            get
            {
                return companyCity;
            }

            set
            {
                companyCity = value;
            }
        }

        public string CompanyCountry
        {
            get
            {
                return companyCountry;
            }

            set
            {
                companyCountry = value;
            }
        }

        public string CompanyPhone
        {
            get
            {
                return companyPhone;
            }

            set
            {
                companyPhone = value;
            }
        }

        public string CompanyEmail
        {
            get
            {
                return companyEmail;
            }

            set
            {
                companyEmail = value;
            }
        }

        public string CompanyWebSite
        {
            get
            {
                return companyWebSite;
            }

            set
            {
                companyWebSite = value;
            }
        }
    }
}
