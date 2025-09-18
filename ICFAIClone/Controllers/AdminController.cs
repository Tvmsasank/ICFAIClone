using FakeItEasy;
using FluentAssertions;
using ICFAIClone.Models;
using ICFAIClone.Models.Entities;
using ICFAIClone.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Threading;
using static System.Collections.Specialized.BitVector32;

namespace ICFAIClone.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;

        private const string adminUser = "admin";
        private const string adminPass = "Admin@123";

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AdminLogin login)
        {
            if (login.Username == adminUser && login.Password == adminPass)
            {
                return RedirectToAction("AdminDashboard");
            }

            ViewBag.Error = "Invalid credentials.";
            return View();
        }

        public IActionResult AdminDashboard()
        {
            var model = new AdminDashboardViewModel
            {
                Enquiries = _adminService.GetAllEnquiries(),
                Students = _adminService.GetAllStudents()
            };
            return View(model);
        }


        [HttpGet]
        public IActionResult EditEnquiry(int id)
        {
            var enquiry = _adminService.GetEnquiryById(id);
            if (enquiry == null)
            {
                return NotFound();
            }
            return View(enquiry);
        }

        [HttpPost]
        public IActionResult EditEnquiry(Enquiry enquiry)
        {
            if (ModelState.IsValid)
            {
                _adminService.UpdateEnquiry(enquiry);
                return RedirectToAction("AdminDashboard");
            }
            return View(enquiry);
        }

        [HttpGet]
        public IActionResult EditStudent(int id)
        {
            var student = _adminService.GetStudentById(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost]
        [Route("Admin/EditStudent")]
        public IActionResult UpdateStudent(Student student)
        {
            _adminService.UpdateStudent(student);
            TempData["UpdateSuccess"] = "Student updated successfully!";
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public IActionResult UpdateAllStudents(AdminDashboardViewModel model)
        {
            foreach (var student in model.Students)
            {
                _adminService.UpdateStudent(student);
            }

            TempData["UpdateSuccess"] = "All students updated successfully!";
            return RedirectToAction("AdminDashboard");
        }

        [HttpGet]

        public IActionResult DeleteEnquiry(int id)
        {
            try
            {
                //call respository/service to delete
                _adminService.DeleteStudent(id);

                TempData["UpdateSuccess"] = "Student deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["UpdateSuccess"] = "Error deleting student:" + ex.Message;
            }
            return RedirectToAction("AdminDashboard"); //wherever your main view is
        }

        [HttpPost]
        public IActionResult DeleteStudent(int id)
        {
            _adminService.DeleteStudent(id);
            return Ok(); // AJAX expects a 200 response
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}


