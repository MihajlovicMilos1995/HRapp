using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HRApi.Models
{
    public class AutoGenHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int AutoGenHistoryId { get; set; }

        public ICollection<Job> Jobs { get; set; }

       // [ForeignKey("Id")]
       // public RegUser RegUser { get; set; }
       // public int RegUserId { get; set; }
        [ForeignKey("JobId") ]
        public Job Job { get; set; }
        public int JobId { get; set; }
    }
}
