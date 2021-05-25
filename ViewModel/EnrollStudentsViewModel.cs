using Microsoft.AspNetCore.Mvc.Rendering;
using MVCUniversity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCUniversity.ViewModel
{
    public class EnrollStudentsViewModel
    {
        public Enrollment Enrollments { get; set; }
        public int Year { get; set; }
        public string Semester { get; set; }  
        public virtual Course Course { get; set; }
        public int CourseId { get; set; }
        [Display(Name = "Selected Students")]
        public IEnumerable<int> SelectedStudents { get; set; }
        public SelectList Courses { get; set; }
        public SelectList StudentsList { get; set; }
    }
}
