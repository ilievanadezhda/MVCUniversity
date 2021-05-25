using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCUniversity.Areas.Identity.Data;
using MVCUniversity.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using MVCUniversity.Data;

namespace MVCUniversity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MVCUniversityContext _context;
        private readonly UserManager<MVCUniversityUSER> userManager;

        public HomeController(ILogger<HomeController> logger, MVCUniversityContext context, UserManager<MVCUniversityUSER> usrMgr)
        {
            _logger = logger;
            _context = context;
            userManager = usrMgr;
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Courses");
            }
            else if (User.IsInRole("Teacher"))
            {
                //Get TeacherId
                var userID = userManager.GetUserId(User);
                MVCUniversityUSER user = await userManager.FindByIdAsync(userID);
                return RedirectToAction("CoursesByTeacher", "Courses", new { id = user.TeacherId });
            }
            else if (User.IsInRole("Student")) {
                var userID = userManager.GetUserId(User);
                MVCUniversityUSER user = await userManager.FindByIdAsync(userID);
                return RedirectToAction("MyEnrollments", "Enrollments", new { id = user.StudentId });
            }
            return View();  
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
