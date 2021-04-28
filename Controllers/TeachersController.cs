﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCUniversity.Data;
using MVCUniversity.Models;
using MVCUniversity.ViewModel;

namespace MVCUniversity.Controllers
{
    public class TeachersController : Controller
    {
        private readonly MVCUniversityContext _context;

        public TeachersController(MVCUniversityContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string teacherDegree, string teacherAcademicRank, string searchStringName, string searchStringSurname)
        {
            //Ако сакам курсевите да ми ги дава во Index.
            //var mVCUniversityContext = _context.Teacher.Include(c => c.CoursesFirst).Include(c => c.CoursesSecond);
            //return View(await mVCUniversityContext.ToListAsync());

            //return View(await _context.Teacher.ToListAsync());

            IQueryable<Teacher> teachers = _context.Teacher.AsQueryable();
            IQueryable<string> degreeQuery = _context.Teacher.OrderBy(m => m.Degree).Select(m => m.Degree).Distinct();
            IQueryable<string> academicRankQuery = _context.Teacher.OrderBy(m => m.AcademicRank).Select(m => m.AcademicRank).Distinct();
            if (!string.IsNullOrEmpty(searchStringName))
            {
                teachers = teachers.Where(s => s.FirstName.Contains(searchStringName));
            }
            if (!string.IsNullOrEmpty(searchStringSurname))
            {
                teachers = teachers.Where(s => s.LastName.Contains(searchStringSurname));
            }
            if (!string.IsNullOrEmpty(teacherDegree))
            {
                teachers = teachers.Where(x => x.Degree == teacherDegree);
            }
            if (!string.IsNullOrEmpty(teacherAcademicRank))
            {
                teachers = teachers.Where(x => x.AcademicRank == teacherAcademicRank);
            }
            var teacherNameDegreeAcademicRankVM = new TeacherNameDegreeAcademicRankViewModel
            {
                Degree = new SelectList(await degreeQuery.ToListAsync()),
                AcademicRank = new SelectList(await academicRankQuery.ToListAsync()),
                Teachers = await teachers.ToListAsync()
            };
            return View(teacherNameDegreeAcademicRankVM);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.Include(c => c.CoursesFirst).Include(c => c.CoursesSecond)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.Include(c => c.CoursesFirst).Include(c => c.CoursesSecond)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.Id == id);
        }
    }
}
