using ICFAIClone.db;
using ICFAIClone.Models;
using ICFAIClone.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ICFAIClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly SqlConnectionHelper _dbHelper;

        
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, SqlConnectionHelper dbHelper, ApplicationDbContext context)
        {
            _logger = logger;
            _dbHelper = dbHelper;
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
         
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
            [HttpPost]
        public IActionResult SubmitEnquiry(Enquiry model)
        {
            string connStr = _dbHelper.GetConnection().ConnectionString;
            bool success = model.SaveToDatabase(connStr);

            if (success)
            {
                TempData["Message"] = "Enquiry submitted successfully!";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Database insert failed.");
            return View(model);
        }

    }
}
