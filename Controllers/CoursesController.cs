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
    public class CoursesController : Controller
    {
        private readonly MVCUniversityContext _context;

        public CoursesController(MVCUniversityContext context)
        {
            _context = context;
        }

        // GET: Courses
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Index(int courseSemester, string courseProgramme, string searchString)
        {
            //var mVCUniversityContext = _context.Course.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher).Include(m => m.Students).ThenInclude(m => m.Student);
            //return View(await mVCUniversityContext.ToListAsync());

            IQueryable<Course> courses = _context.Course.AsQueryable();
            IQueryable<string> programmeQuery = _context.Course.OrderBy(m => m.Programme).Select(m => m.Programme).Distinct();
            IQueryable<int> semestarQuery = _context.Course.OrderBy(m => m.Semester).Select(m => m.Semester).Distinct();
            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(courseProgramme))
            {
                courses = courses.Where(x => x.Programme == courseProgramme);
            }
            if(courseSemester != 0)
            {
                courses = courses.Where(x => x.Semester == courseSemester);
            }
            courses = courses.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher).Include(m => m.Students).ThenInclude(m => m.Student);
            var courseSemesterProgrammeVM = new CourseSemesterProgrammeViewModel
            {
                Semester = new SelectList(await semestarQuery.ToListAsync()),
                Programme = new SelectList(await programmeQuery.ToListAsync()),
                Courses = await courses.ToListAsync()
            };
            return View(courseSemesterProgrammeVM);
        }

        // GET: Courses/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher).Include(m => m.Students).ThenInclude(m => m.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName");
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Course.Where(m => m.Id == id).Include(m => m.Students).First();

            if (course == null)
            {
                return NotFound();
            }

            var students = _context.Student.AsEnumerable();
            students = students.OrderBy(s => s.FullName);
            CourseStudentEditViewModel viewmodel = new CourseStudentEditViewModel
            { 
                Course = course,
                StudentList = new MultiSelectList(students, "Id", "FullName"),
                SelectedStudents = course.Students.Select(sa => sa.StudentId)
            };

            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.SecondTeacherId);
            return View(viewmodel);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, CourseStudentEditViewModel viewmodel)
        {
            if (id != viewmodel.Course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Course);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> listStudents = viewmodel.SelectedStudents;
                    IQueryable<Enrollment> toBeRemoved = _context.Enrollment.Where(s => !listStudents.Contains(s.StudentId) && s.CourseId == id);
                    _context.Enrollment.RemoveRange(toBeRemoved);
                    IEnumerable<int> existStudents = _context.Enrollment.Where(s => listStudents.Contains(s.StudentId) && s.CourseId == id).Select(s => s.StudentId);
                    IEnumerable<int> newStudents = listStudents.Where(s => !existStudents.Contains(s));
                    foreach (int studentId in newStudents)
                        _context.Enrollment.Add(new Enrollment { StudentId = studentId, CourseId = id });

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(viewmodel.Course.Id))
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
            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", viewmodel.Course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", viewmodel.Course.SecondTeacherId);
            return View(viewmodel);
        }

        /*public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            ViewData["FirstTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Set<Teacher>(), "Id", "FullName", course.SecondTeacherId);
            return View(course);
        }*/


        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher).Include(m => m.Students).ThenInclude(m => m.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }

        //My Courses
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CoursesByTeacher(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var teacher = await _context.Teacher.FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.Message = teacher;

            if (teacher == null)
            {
                return NotFound();
            }
            var mVCUniversityContext = _context.Course.Where(m => (m.FirstTeacherId == id) || (m.SecondTeacherId == id)).Include(c => c.FirstTeacher).Include(c => c.SecondTeacher); //.Include(m => m.Students).ThenInclude(m => m.Student);
            return View(await mVCUniversityContext.ToListAsync());
        }
    }
}
