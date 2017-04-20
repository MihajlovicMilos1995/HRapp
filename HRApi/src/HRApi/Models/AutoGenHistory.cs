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
        public class AutoGenWorkHistory
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
            public int AutoGenId { get; set; }

            public ICollection<JobPosition> Jobs { get; set; }

            [ForeignKey("RegUserId")]
            public RegUser RegUser { get; set; }
            public int RegUserId { get; set; }

            [ForeignKey("JobPositionId")]
            public Jobs JobPos { get; set; }
            public int JobPositionId { get; set; }
        }
    }
}
