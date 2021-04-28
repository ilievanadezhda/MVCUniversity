using Microsoft.AspNetCore.Mvc.Rendering;
using MVCUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCUniversity.ViewModel
{
    public class StudentNameIdViewModel
    {
        public IList<Student> Students { get; set; }
        //public SelectList StudentId { get; set; }
        public string SearchStudentId { get; set; }
        public string SearchStringName { get; set; }
        public string SearchStringSurname { get; set; }
    }
}
