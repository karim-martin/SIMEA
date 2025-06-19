using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIMEA.Models
{
    public class IdentityExtendUser : IdentityUser
    {
        [Required, MaxLength(40), Display(Name = "Full name*")]
        public string FullName { get; set; }

        public string AccountStatus { get; set; }

        public ICollection<IdentityUserRole<string>> UserRoles { get; set; }

        public IdentityExtendUser()
        {
            UserRoles = new List<IdentityUserRole<string>>();
        }
    }

    public class UserRole : IdentityRole
    {
        public string Description { get; set; }
    }

    public class UserViewModel
    {
        public string Id { get; set; }

        [Required, Display(Name = "Full Name*")]
        public string FullName { get; set; }

        [Required, EmailAddress, Display(Name = "UserName*")]
        public string UserName { get; set; }

        [Required, EmailAddress, Display(Name = "Email")]
        public string Email { get; set; }

        [Required, Display(Name = "Account Status")]
        public string AccountStatus { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public List<RoleViewModel> Roles { get; set; }
        
        public List<string> SelectedRoles { get; set; }
    }

    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}
