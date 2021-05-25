using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCUniversity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MVCUniversity.Areas.Identity.Data;


namespace MVCUniversity.Data
{
    public class MVCUniversityContext : IdentityDbContext<MVCUniversityUSER>
    {
        public MVCUniversityContext (DbContextOptions<MVCUniversityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Ovie 2 gi nema vo prezentacijata ama neka stojat ovde
            builder.Entity<Course>().HasOne(p => p.FirstTeacher).WithMany(p => p.CoursesFirst).HasForeignKey(p => p.FirstTeacherId);
            builder.Entity<Course>().HasOne(p => p.SecondTeacher).WithMany(p => p.CoursesSecond).HasForeignKey(p => p.SecondTeacherId);
            //Ova e dodadeno od prezentacijata
            base.OnModelCreating(builder);
        }

        public DbSet<MVCUniversity.Models.Course> Course { get; set; }

        public DbSet<MVCUniversity.Models.Student> Student { get; set; }

        public DbSet<MVCUniversity.Models.Teacher> Teacher { get; set; }
        public DbSet<MVCUniversity.Models.Enrollment> Enrollment { get; set; }

    }
}
