using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.UI.WebControls;

namespace EC.Models
{
    public class AdminModels
    {
        [Key]
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       
        [Display(Name = "Password Confirmation")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and password confirmation must match.")]
        public string PasswordConfirmation { get; set; }

 
        [Required]
        [Display(Name = "User Roles")]
        public List<string> Roles { get; set; }

        
    }



    public class RolesDataSet
    {
        public string Id { get; set; }
        public string Role { get; set; }
    }

   
    public class UserView
    {
        public string Email { get; set; }

        public string Phone { get; set; }

        public string UserName { get; set; }
    }


    public class UserRoles
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public List<string> Roles { get; set; }
    }
    public class ChangePassword
    {
        public string Email { get; set; }

        [Required]
        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Password Confirmation")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and password confirmation must match.")]
        public string PasswordConfirmation { get; set; }
    }
}