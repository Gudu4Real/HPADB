using BataCMS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class TransactionDetailViewModel
    {
        public Transaction Transaction { get; set; }
        public HPAFacility RentalAsset { get; set; }
        public ApplicationUser VendorUser { get; set; }
        public ApplicationUser Server { get; set; }
        public MemberSubscription Lease { get; set; }

    }
}
