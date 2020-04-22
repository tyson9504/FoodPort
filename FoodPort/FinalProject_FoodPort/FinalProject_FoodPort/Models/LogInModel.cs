using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject_FoodPort.Models
{
    public class LogInModel
    {
        [Display(Name="User ID:")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter Your Phone Number")]
        [Required(ErrorMessage="*")]
        public String UserID { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "*")]
        
        [DataType(DataType.Password)]
        public String Password { get; set; }
        [Display(Name = "User Type")]
        [Required(ErrorMessage = "*")]
        public String UserType { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}