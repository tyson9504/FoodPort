using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject_FoodPort.Models
{
    public class AdminModel
    {
        [Display(Name="Admin ID")]
        public int AdminID { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "*")]
        [StringLength(30, ErrorMessage = "Max 30 Chars")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[$@!%*#?&])[A-Za-z\\d@$!%*#?&\\s]{6,}$", ErrorMessage = "Invalid Password Format! (Should be Alphanumeric and contain ateast one special character and minimum length of 6 characters)")] 
        public String AdminPasswrod { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "*")]
        [StringLength(30, ErrorMessage = "Max 30 Chars")]
        public String AdminName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "*")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Only 10 Chars")]
        [System.Web.Mvc.Remote("CheckMobileNumber", "Admin", ErrorMessage = "Already Exists")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Invalid Format!")]
        public String AdminPhoneNumber { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid Format")]
        [StringLength(50, ErrorMessage = "Max 50 Chars")]
        public String AdminEmailID { get; set; }
    }
}