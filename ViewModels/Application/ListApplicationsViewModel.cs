﻿using BataCMS.Data.Models;
using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BataCMS.ViewModels
{
    public class ListApplicationsViewModel
    {
        public IEnumerable<MemberApplication> Applications { get; set; }

    }
}
