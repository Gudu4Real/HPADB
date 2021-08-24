using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.Models
{
    public class MemberSubscription
    {
        public int MemberSubscriptionId { get; set; }

        public string UserId { get; set; }

        public int HPAFacilityId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime To { get; set; }

/*        public virtual HPAFacility HPAFacility { get; set; }*/


    }
}
