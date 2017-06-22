using HRApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRApi.Models.UserViewModel
{
    public class UserDTO
    {
        public string RegUserName { get; set; }
        public string RegUserLastName { get; set; }
        public string RegUserCity { get; set; }
        public string RegUserCountry { get; set; }
        public bool LocationChange { get; set; }
        public JobType RegUserPartFull { get; set; }
        public string WorkXp { get; set; }
        public string RegUserKeyword { get; set; }

    }
}
