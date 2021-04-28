using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVCUniversity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCUniversity.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MVCUniversityContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MVCUniversityContext>>()))
            {
                
                if (context.Course.Any() || context.Teacher.Any() || context.Student.Any())
                {
                    return;   
                }

                context.Teacher.AddRange(
                    new Teacher { FirstName = "Даниел", LastName = "Денковски", Degree = "Доктор на науки" },
                    new Teacher { FirstName = "Перо", LastName = "Латковски", Degree = "Доктор на науки" },
                    new Teacher { FirstName = "Владимир", LastName = "Атанасовски", Degree = "Доктор на науки" },
                    new Teacher { FirstName = "Валентин", LastName = "Раковиќ", Degree = "Доктор на науки" }
                );
                context.SaveChanges();

                context.Student.AddRange(
                    new Student { StudentId = "17/2018", FirstName = "Надежда", LastName = "Илиева", EnrollmentDate = DateTime.Parse("2018-09-01") },
                    new Student { StudentId = "1/2018", FirstName = "Елена", LastName = "Стојановска", EnrollmentDate = DateTime.Parse("2018-09-01") },
                    new Student { StudentId = "150/2018", FirstName = "Никола", LastName = "Трифуновски", EnrollmentDate = DateTime.Parse("2018-09-01") },
                    new Student { StudentId = "37/2018", FirstName = "Лара", LastName = "Спасовска", EnrollmentDate = DateTime.Parse("2018-09-01") },
                    new Student { StudentId = "45/2018", FirstName = "Стефан", LastName = "Поповски", EnrollmentDate = DateTime.Parse("2018-09-01") },
                    new Student { StudentId = "11/2018", FirstName = "Ана", LastName = "Лазаревска", EnrollmentDate = DateTime.Parse("2018-09-01") },
                    new Student { StudentId = "19/2018", FirstName = "Давид", LastName = "Иванов", EnrollmentDate = DateTime.Parse("2018-09-01") },
                    new Student { StudentId = "304/2017", FirstName = "Никола", LastName = "Велков", EnrollmentDate = DateTime.Parse("2017-09-01") }
                );
                context.SaveChanges();

                context.Course.AddRange(
                    new Course
                    {
                        Title = "Развој на серверски WEB aпликации",
                        Credits = 6,
                        Semester = 6,
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Даниел" && d.LastName == "Денковски").Id,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Перо" && d.LastName == "Латковски").Id
                    },

                    new Course
                    {
                        Title = "Мобилни сервиси со Андроид програмирање",
                        Credits = 6,
                        Semester = 6,
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Даниел" && d.LastName == "Денковски").Id,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Перо" && d.LastName == "Латковски").Id
                    },

                    new Course
                    {
                        Title = "Основи на WEB програмирање",
                        Credits = 6,
                        Semester = 5,
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Владимир" && d.LastName == "Атанасовски").Id,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Валентин" && d.LastName == "Раковиќ").Id
                    },

                    new Course
                    {
                        Title = "Андроид програмирање",
                        Credits = 6,
                        Semester = 5,
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Даниел" && d.LastName == "Денковски").Id,
                        SecondTeacherId = context.Teacher.Single(d => d.FirstName == "Владимир" && d.LastName == "Атанасовски").Id
                    },

                    new Course
                    {
                        Title = "Телекомуникациски мрежи",
                        Credits = 6,
                        Semester = 6,
                        FirstTeacherId = context.Teacher.Single(d => d.FirstName == "Владимир" && d.LastName == "Атанасовски").Id,
                    }

                );
                context.SaveChanges();

                context.Enrollment.AddRange(
                    new Enrollment { StudentId = 1, CourseId = 1},
                    new Enrollment { StudentId = 1, CourseId = 2 },
                    new Enrollment { StudentId = 1, CourseId = 4 },
                    new Enrollment { StudentId = 2, CourseId = 1 },
                    new Enrollment { StudentId = 2, CourseId = 3 },
                    new Enrollment { StudentId = 2, CourseId = 5 },
                    new Enrollment { StudentId = 3, CourseId = 4 },
                    new Enrollment { StudentId = 4, CourseId = 2 },
                    new Enrollment { StudentId = 4, CourseId = 3 },
                    new Enrollment { StudentId = 5, CourseId = 1 },
                    new Enrollment { StudentId = 6, CourseId = 4 },
                    new Enrollment { StudentId = 7, CourseId = 1 },
                    new Enrollment { StudentId = 7, CourseId = 2 },
                    new Enrollment { StudentId = 7, CourseId = 3 }
                );

                context.SaveChanges();
            }
        }
    }
}
