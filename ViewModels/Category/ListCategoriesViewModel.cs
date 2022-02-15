using BataCMS.Data.Models;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HPADB.ViewModels
{
    public class ListCategoriesViewModel
    {
        public IEnumerable<Category> Sectors { get; set; }

        public Dictionary<string, decimal> FacilityPricing{ get; set; }

        public int AvailableFacilities { get; set; }
    }
}
