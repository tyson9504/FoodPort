using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace FinalProject_FoodPort.Models
{
    public class RestaurantModel
    {
        [Display(Name = "Restaurant ID")]
        public int RestaurantID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "Max 30 Chars")]
        public String RestaurantName { get; set; }

        [Display(Name = "Locality")]
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "Max 30 Chars")]
        public String RestaurantLocality { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "Max 30 Chars")]
        public String RestaurantCity { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "Max 30 Chars")]
        public String RestaurantState { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "*")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Only 10 Chars")]
        [System.Web.Mvc.Remote("CheckMobileNumberres", "Admin", ErrorMessage = "Already Exists")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Invalid Format!")]
        public String RestaurantPhoneNumber { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid Format")]
        [StringLength(50, ErrorMessage = "Max 30 Chars")]
        public String RestaurantEmailID { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "Max 50 Chars")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[$@!%*#?&])[A-Za-z\\d@$!%*#?&\\s]{6,}$", ErrorMessage = "Invalid Password Format! (Should be Alphanumeric and contain ateast one special character and minimum length of 6 characters)")] 
        public String RestaurantPassWord { get; set; }

        [Display(Name = "Open Time")]
        public String OpenTime { get; set; }

        [Display(Name = "Close Time")]
        public String CloseTime { get; set; }

        [Display(Name = "Status")]
        public String Status { get; set; }

        [Display(Name = "Image")]
        public String Image { get; set; }

    }
}