using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdvert.Web.Models.Accounts
{
    public class ConfirmModel
    {
        [Required(ErrorMessage ="Email is required")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }
    }


    public class ForgotpasswordModel
    {
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }
    }

    public class ResetpasswordModel
    {
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }

        public string Password { get; set; }
    }

}
