using Microsoft.AspNetCore.Http;
using MVCUniversity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCUniversity.ViewModel
{
    public class StudentViewModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Student Id")]
        public string StudentId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Enrollment Date")]
        public DateTime? EnrollmentDate { get; set; }

        [Display(Name = "Acquired Credits")]
        public int? AcquiredCredits { get; set; }

        [Range(1, 8)]
        [Display(Name = "Current Semestar")]
        public int? CurrentSemester { get; set; }

        [StringLength(25)]
        [Display(Name = "Education Level")]
        public string EducationLevel { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? Picture { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        public ICollection<Enrollment> Courses { get; set; }

    }
}
