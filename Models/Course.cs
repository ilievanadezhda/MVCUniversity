using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVCUniversity.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public int Credits { get; set; }

        [Required]
        [Range(1,8)]
        public int Semester { get; set; }

        [StringLength(100)]
        public string Programme { get; set; }

        [StringLength(25)]
        [Display(Name = "Education Level")]
        public string EducationLevel { get; set; }

        [Display(Name = "First Teacher")]
        //[ForeignKey("FirstTeacher"), Column(Order = 0)]
        public int? FirstTeacherId { get; set; }

        [Display(Name = "Second Teacher")] 
        //[ForeignKey("SecondTeacher"), Column(Order = 1)]
        public int? SecondTeacherId { get; set; }

        [ForeignKey("FirstTeacherId")]
        [Display(Name = "First Teacher")]
        public virtual Teacher FirstTeacher { get; set; }

        [ForeignKey("SecondTeacherId")]
        [Display(Name = "Second Teacher")]
        public virtual Teacher SecondTeacher { get; set; }

        public ICollection<Enrollment> Students { get; set; }

    }
}
