using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.Data.Models
{
    public class MemberCertificate
    {
        public int MemberCertificateId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ValidFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ValidTo { get; set; }

        public int HPAFacilityId { get; set; }

        public virtual MemberUser MemberUser { get; set; }
    }
}
