using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject_FoodPort.Models
{
    public class Summary
    {
        [Display(Name="Restaurant Name")]
        public string RestaurantName { get; set; }
        [Display(Name="Total Items")]
        public int TotalItems { get; set; }
        [Display(Name="Total Amount")]
        public decimal TotalAmount { get; set; }
    }
}