using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCUniversity.Models;

namespace MVCUniversity.Data
{
    public class MVCUniversityContext : DbContext
    {
        public MVCUniversityContext (DbContextOptions<MVCUniversityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Course>().HasOne(p => p.FirstTeacher).WithMany(p => p.CoursesFirst).HasForeignKey(p => p.FirstTeacherId);
            builder.Entity<Course>().HasOne(p => p.SecondTeacher).WithMany(p => p.CoursesSecond).HasForeignKey(p => p.SecondTeacherId);
        }

        public DbSet<MVCUniversity.Models.Course> Course { get; set; }

        public DbSet<MVCUniversity.Models.Student> Student { get; set; }

        public DbSet<MVCUniversity.Models.Teacher> Teacher { get; set; }
        public DbSet<MVCUniversity.Models.Enrollment> Enrollment { get; set; }
    }
}
