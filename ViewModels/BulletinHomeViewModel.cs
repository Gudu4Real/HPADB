using BataCMS.Data.Models;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class BulletinHomeViewModel
    {
        public IEnumerable<HPAFacility> Companies { get; set; }

        public string SectorName { get; set; }
    }
}
