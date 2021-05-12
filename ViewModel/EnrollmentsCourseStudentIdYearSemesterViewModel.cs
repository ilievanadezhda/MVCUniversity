using Microsoft.AspNetCore.Mvc.Rendering;
using MVCUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCUniversity.ViewModel
{
    public class EnrollmentsCourseStudentIdYearSemesterViewModel
    {
        public IList<Enrollment> Enrollments { get; set; }
        public SelectList Year { get; set; }
        public SelectList Semester { get; set; }
        public int EnrollmentsYear { get; set; }
        public string EnrollmentsSemester { get; set; }
        public string SearchStringCourse { get; set; }
        public string SearchStringStudentId { get; set; }

    }
}
