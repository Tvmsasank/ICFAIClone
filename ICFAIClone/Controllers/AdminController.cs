using ICFAIClone.db;
using ICFAIClone.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ICFAIClone.Controllers
{
    public class AdminController : Controller
    {
        private readonly SqlConnectionHelper _dbHelper;

        private const string adminUser = "admin";
        private const string adminPass = "Admin@123";

        public AdminController(SqlConnectionHelper dbHelper)
        {
            _dbHelper = dbHelper;
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
            List<Enquiry> enquiries = GetAllEnquiries();
            return View(enquiries);
        }

        private List<Enquiry> GetAllEnquiries()
        {
            var list = new List<Enquiry>();

            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT Name, Email, Phone, Address FROM Enquiries", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Enquiry
                        {
                            Name = reader.GetString(0),
                            Email = reader.GetString(1),
                            Phone = reader.GetString(2),
                            Address = reader.GetString(3)
                        });
                    }
                }
            }

            return list;
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
