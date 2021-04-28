using Microsoft.AspNetCore.Mvc.Rendering;
using MVCUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCUniversity.ViewModel
{
    public class TeacherNameDegreeAcademicRankViewModel
    {
        public IList<Teacher> Teachers { get; set; }
        public SelectList Degree { get; set; }
        public SelectList AcademicRank { get; set; }
        public string TeacherDegree { get; set; }
        public string TeacherAcademicRank { get; set; }
        public string SearchStringName { get; set; }
        public string SearchStringSurname { get; set; }
    }
}
