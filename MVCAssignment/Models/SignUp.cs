using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCAssignment.Models
{
    public class SignUp
    {

        [DisplayName("UserName")]
        [Required(ErrorMessage = "The UserName is required.")]
        public string UserName { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "The Password is required.")]
        public string Password { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "The Email is required.")]
        public string Email { get; set; }
    }
}