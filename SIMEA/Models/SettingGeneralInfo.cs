using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SIMEA.Models
{
    public class SettingGeneralInfo
    {
        public int Id { get; set; }

        [Required, Display(Name = "Organization Name")]
        public string OrganizationName { get; set; }

        [MaxLength(9)]
        [Required, Display(Name = "Business TRN")]
        public string BusinessTRN { get; set; }

        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required, Display(Name = "Telephone")]
        public string Telephone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Fax")]
        public string Fax { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Parish")]
        public string Parish { get; set; }

        [Required, Display(Name = "License Key"), StringLength(74, MinimumLength = 74, ErrorMessage = "License Key must be exactly 74 characters.")]
        public string LicenseKey { get; set; }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////
        /// </summary>
        [Display(Name = "Sunday")]
        public bool Sunday { get; set; }

        [Display(Name = "Monday")]
        public bool Monday { get; set; }

        [Display(Name = "Tuesday")]
        public bool Tuesday { get; set; }

        [Display(Name = "Wednesday")]
        public bool Wednesday { get; set; }

        [Display(Name = "Thursday")]
        public bool Thursday { get; set; }

        [Display(Name = "Friday")]
        public bool Friday { get; set; }

        [Display(Name = "Saturday")]
        public bool Saturday { get; set; }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>

        public string UserCreated { get; set; }

        public string UserModified { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateCreated { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DateModified { get; set; } = DateTime.Now;
    }
}
