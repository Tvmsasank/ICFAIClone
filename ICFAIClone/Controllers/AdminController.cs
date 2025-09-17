using ICFAIClone.Models;
using ICFAIClone.Services;
using Microsoft.AspNetCore.Mvc;

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
            var enquiries = _adminService.GetAllEnquiries();
            return View(enquiries);
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
