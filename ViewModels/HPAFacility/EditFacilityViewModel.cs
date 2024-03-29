﻿using COHApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace COHApp.ViewModels
{
    public class EditFacilityViewModel : CreateFacilityViewModel
    {
        public int FacilityId { get; set; }

        public string ExistingImagePath {get; set;}

        public List<Image> ExistingImages { get; set; }
    }
}
