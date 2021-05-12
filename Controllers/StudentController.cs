using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCUniversity.Data;
using MVCUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCUniversity.Controllers
{
    public class StudentController : Controller
    {
        private readonly MVCUniversityContext _context;

        public StudentController(MVCUniversityContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Enrollments(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = await _context.Student.FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.Message = student;
            if (student == null)
            {
                return NotFound();
            }
            var mVCUniversityContext = _context.Enrollment.Where(m => m.StudentId == id).Include(m => m.Student).Include(m => m.Course);
            return View(await mVCUniversityContext.ToListAsync());
        }
        public async Task<IActionResult> EditEnrollment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            int studentId = enrollment.StudentId;
            int courseId = enrollment.CourseId;
            var course = await _context.Course.FindAsync(courseId);
            ViewBag.Course = course;
            var student = await _context.Student.FindAsync(studentId);
            ViewBag.Student = student;
            return View(enrollment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEnrollment(int id, [Bind("Id,StudentId,CourseId,Semester,Year,Grade,SeminarUrl,ProjectUrl,ExamPoints,SeminarPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (id != enrollment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Enrollments));
            }
            return View(enrollment);
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.Id == id);
        }
    }
}

