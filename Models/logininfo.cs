using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCSampleProject.Models
{
    public class logininfo
    {
        [Display(Name = "Username")]
        [MinLength(6, ErrorMessage = "Username must at least 6 characters")]
        [Required(ErrorMessage = "User name cannot be blank")]
        [RegularExpression(@"^[A-Xa-z 0-9]*$", ErrorMessage = "Your username cannot contain special characters")]
        public string username { get; set; }
        [MinLength(6, ErrorMessage = "Password must at least 6 characters")]
        [Required(ErrorMessage = "Password cannot be empty")]
        [Display(Name = "Password")]
        public string password { get; set; }
    }
}