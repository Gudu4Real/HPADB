﻿using BataCMS.Data.Models;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.Data.ViewModels
{
    public class UnitItemListViewModel
    {
        public IEnumerable<HPAFacility> UnitItems { get; set; }

        public string CurrentCategory { get; set; }
    }
}
