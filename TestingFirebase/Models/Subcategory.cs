﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TestingFirebase.HelperClasses;

namespace TestingFirebase.Models
{
    public class Subcategory
    {
        [Key]
        public string Key { get; set; }//primary key. A sequence is created and incremented by default
       
        [Required(ErrorMessage = "Title can't be empty!")]
        public string Title { get; set; }

        //[Required(ErrorMessage = "Color can't be empty!")]
        public string Color { get; set; }
        
        [Required(ErrorMessage = "Evaluation can't be empty!")]
        public string Evaluation { get; set; }

        [Required(ErrorMessage = "Medications can't be empty!")]
        public string Medications { get; set; }

        [Required(ErrorMessage = "Management can't be empty!")]
        public string Management { get; set; }

        [Required(ErrorMessage = "Symptoms can't be empty!")]
        public string Signs{ get; set; }

        [Required(ErrorMessage = "References can't be empty!")]
        public string References { get; set; }

        //[DataType(DataType.ImageUrl)]
        //[Display(Name = "Image Link")]
        //[Required(ErrorMessage = "Image Link can't be empty!")]
        //public string ImageUrl { get; set; }

        //for linking to Main Kategory
        [Display(Name = "Main Category ID")]
        [Required(ErrorMessage = "Main Category can't be empty!")]
        [StringRange(AllowableValues = new[] { "c1", "c2", "c3", "c4", "c5" }, ErrorMessage = "Main Category should be 'c1' thru 'c5'.")]
        public string SubId { get; set; }

        //[ForeignKey("SubId")]
        //public virtual MainCategory MainCategory { get; set; }
    }
}
