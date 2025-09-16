using ICFAIClone.db;
using ICFAIClone.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

public class StudentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly SqlConnectionHelper _dbHelper;

    // ✅ Single constructor with both dependencies
    public StudentController(ApplicationDbContext context, SqlConnectionHelper dbHelper)
    {
        _context = context;
        _dbHelper = dbHelper;
    }

    // ✅ Create Student (GET)
    public IActionResult Create() => View();

    // ✅ Create Student (POST)

    [HttpPost]
    public IActionResult Create(Student student)
    {
        if (student.Course != null)
        {
            string connStr = _dbHelper.GetConnection().ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("InsertStudent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Course", student.Course);
                cmd.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@EnrollmentNumber", student.EnrollmentNumber);
                cmd.Parameters.AddWithValue("@FullName", student.FullName);
                cmd.Parameters.AddWithValue("@Fees", student.Fees);
                cmd.Parameters.AddWithValue("@IsActive", student.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ViewBag.Message = "Student inserted successfully!";
        }

        return View("Create"); // stays on the same page
    }

    // ✅ List Students
    public IActionResult Index()
    {
        var students = _context.Students.ToList(); // ✅ List of students
        return View(students); // ✅ Correct type
    }

    // ✅ Login Page (GET)
    public IActionResult Login()
    {
        return View();
    }

    // ✅ Login Logic (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(Student student)
    {
        bool isValid = ValidateStudentLogin(student.Username, student.Password);

        if (isValid)
        {
            HttpContext.Session.SetString("StudentUsername", student.Username);
            return RedirectToAction("Dashboard");
        }

        ViewBag.Message = "Invalid username or password";
        return View();
    }

    // ✅ Validate Login via Stored Procedure
    private bool ValidateStudentLogin(string username, string password)
    {
        using (var connection = new SqlConnection(_dbHelper.GetConnection().ConnectionString))
        {
            using (var command = new SqlCommand("ValidateStudentLogin", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                var statusParam = new SqlParameter("@Status", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(statusParam);

                connection.Open();
                command.ExecuteNonQuery();

                return (int)statusParam.Value == 1;
            }
        }
    }

    // ✅ Dashboard View
    public IActionResult Dashboard()
    {
        var username = HttpContext.Session.GetString("StudentUsername");

        if (string.IsNullOrEmpty(username))
            return RedirectToAction("Login");

        var student = _context.Students.FirstOrDefault(s => s.Username == username);

        if (student == null)
        {
            ViewBag.Message = "Student not found.";
            return View(new List<Student>());
        }

        ViewBag.Username = username;
        return View(new List<Student> { student });
    }


    // ✅ Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
