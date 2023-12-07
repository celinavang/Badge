﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Badge.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {

        [PersonalData]
        [DisplayName("Fornavn")]
        public string? FName { get; set; }
        [PersonalData]
        [DisplayName("Fornavn")]
        public string? LName { get; set; }
        [PersonalData]
        [DisplayName("Profil Billede")]
        public string? AppUImageData { get; set; }

        [NotMapped]
        public string FullName { get { return FName + " " + LName; } }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
