using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public ICollection<Job> Jobs { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int CompanyId { get; set; }

        [DisplayName("Name")]
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

        [DisplayName("Descrition")]
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

        [DisplayName("City")]
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

        [DisplayName("Country")]
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

        [DisplayName("Phone number")]
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

        [DisplayName("Email")]
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

        [DisplayName("Site")]
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
