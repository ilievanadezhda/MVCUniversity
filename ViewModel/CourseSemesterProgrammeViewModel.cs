using Microsoft.AspNetCore.Mvc.Rendering;
using MVCUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCUniversity.ViewModel
{
    public class CourseSemesterProgrammeViewModel
    {
        public IList<Course> Courses { get; set; }
        public SelectList Semester { get; set; }
        public SelectList Programme { get; set; }
        public int CourseSemester { get; set; }
        public string CourseProgramme { get; set; }
        public string SearchString { get; set; }
    }
}
