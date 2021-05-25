using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCUniversity.Data;
using MVCUniversity.Models;
using MVCUniversity.ViewModel;

namespace MVCUniversity.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly MVCUniversityContext _context;

        public EnrollmentsController(MVCUniversityContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int enrollmentsYear, string enrollmentsSemester, string searchStringCourse, string searchStringStudentId)
        {
            //var mVCUniversityContext = _context.Enrollment.Include(e => e.Course).Include(e => e.Student);
            //return View(await mVCUniversityContext.ToListAsync());

            IQueryable<Enrollment> enrollments = _context.Enrollment.AsQueryable();
            IQueryable<int?> yearQuery = _context.Enrollment.OrderBy(m => m.Year).Select(m => m.Year).Distinct();
            IQueryable<string> semesterQuery = _context.Enrollment.OrderBy(m => m.Semester).Select(m => m.Semester).Distinct();
            if (!string.IsNullOrEmpty(searchStringCourse))
            {
                enrollments = enrollments.Where(s => s.Course.Title.Contains(searchStringCourse));
            }
            if (!string.IsNullOrEmpty(searchStringStudentId))
            {
                enrollments = enrollments.Where(s => s.Student.StudentId.Contains(searchStringStudentId));
            }
            if (enrollmentsYear != 0)
            {
                enrollments = enrollments.Where(x => x.Year == enrollmentsYear);
            }
            if (!string.IsNullOrEmpty(enrollmentsSemester))
            {
                enrollments = enrollments.Where(x => x.Semester == enrollmentsSemester);
            }

            enrollments = enrollments.Include(c => c.Student).Include(c => c.Course);
            var enrollmentsCourseStudentIdYearSemesterVM = new EnrollmentsCourseStudentIdYearSemesterViewModel
            {
                Year = new SelectList(await yearQuery.ToListAsync()),
                Semester = new SelectList(await semesterQuery.ToListAsync()),
                Enrollments = await enrollments.ToListAsync()
            };
            return View(enrollmentsCourseStudentIdYearSemesterVM);
        }

        // GET: Enrollments/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title");
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,StudentId,CourseId,Semester,Year,Grade,SeminarUrl,ProjectUrl,ExamPoints,SeminarPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,CourseId,Semester,Year,Grade,SeminarUrl,ProjectUrl,ExamPoints,SeminarPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Title", enrollment.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "FullName", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Enrolled students in my course
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> EnrolledStudentsByCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Course.FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.Message = course;
            if (course == null)
            {
                return NotFound();
            }
            var mVCUniversityContext = _context.Enrollment.Where(m => m.CourseId == id).Include(m => m.Student).Include(m => m.Course);
            return View(await mVCUniversityContext.ToListAsync());
        }

        //Teacher edits an enrollment
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> EditEnrollmentByTeacher(int? id)
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
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> EditEnrollmentByTeacher(int id, [Bind("Id,StudentId,CourseId,Semester,Year,Grade,SeminarUrl,ProjectUrl,ExamPoints,SeminarPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
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
                return RedirectToAction(nameof(EnrolledStudentsByCourse), new { id = enrollment.CourseId});
            }
            return View(enrollment);
        }

        //My enrollments (for students)
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> MyEnrollments(int? id)
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

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EditEnrollmentByStudent(int? id)
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
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EditEnrollmentByStudent(int id, [Bind("Id,StudentId,CourseId,Semester,Year,Grade,SeminarUrl,ProjectUrl,ExamPoints,SeminarPoints,ProjectPoints,AdditionalPoints,FinishDate")] Enrollment enrollment)
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
                return RedirectToAction(nameof(MyEnrollments), new { id = enrollment.StudentId});
            }
            return View(enrollment);
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.Id == id);
        }

        public async Task<IActionResult> EnrollStudents()
        {
            IQueryable<Course> courses = _context.Course;
            IEnumerable<Student> students = _context.Student;

            EnrollStudentsViewModel enrollStudentsViewModel = new EnrollStudentsViewModel
            {
                Courses = new SelectList(await courses.ToListAsync(), "Id", "Title"),
                StudentsList = new SelectList(students.OrderBy(s => s.FullName).ToList(), "Id", "FullName"),
            };
            return View(enrollStudentsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollStudents(EnrollStudentsViewModel enrollStudentsViewModel)
        {
            var course = await _context.Course.FirstOrDefaultAsync(c => c.Id == enrollStudentsViewModel.CourseId);
            if (course == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                foreach (int studentId in enrollStudentsViewModel.SelectedStudents)
                {
                    Enrollment enroll = await _context.Enrollment
                        .FirstOrDefaultAsync(c => c.CourseId == enrollStudentsViewModel.CourseId && c.StudentId == studentId);
                    if (enroll == null)
                    {

                        _context.Add(new Enrollment { StudentId = studentId, CourseId = enrollStudentsViewModel.CourseId, Year = enrollStudentsViewModel.Enrollments.Year, Semester = enrollStudentsViewModel.Enrollments.Semester });
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
