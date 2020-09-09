﻿using BataCMS.Data.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BataCMS.Data.Models
{
    public class unitItem
    {
        [Required]
        public int unitItemId { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:n2}")]
        public decimal Price { get; set; }

        public bool InStock { get; set; }

        public DateTime DateModified { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        public string ImageUrl { get; set; }


        public string OptionFormData { get; set; }

    }
}
    