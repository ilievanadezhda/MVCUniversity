using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MVCUniversity.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the MVCUniversityUSER class
    public class MVCUniversityUSER : IdentityUser
    {
        public int? StudentId { get; set; }

        public int? TeacherId { get; set; }
    }
}
