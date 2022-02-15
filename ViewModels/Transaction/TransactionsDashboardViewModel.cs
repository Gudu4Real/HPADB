using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class TransactionsDashboardViewModel
    {
        public HPAFacility HPAFacility { get; set; }

        public Dictionary<string, int> SaleTypes { get; set; }

        public Dictionary<string, decimal> FacilityPricing { get; set; }

        public int UserCount { get; set; }

        public decimal TotalSales { get; set; }

        public decimal OpCosts { get; set; }

        public decimal AssetValue2021 { get; set; }

        public decimal AssetValue2022 { get; set; }



    }
}