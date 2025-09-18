using ICFAIClone.Models.Entities;
using ICFAIClone.Services;
using Microsoft.AspNetCore.Mvc;

public class StudentController : Controller
{
    private readonly StudentService _studentService;

    public StudentController(StudentService studentService)
    {
        _studentService = studentService;
    }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Student student)
    {
        _studentService.InsertStudent(student); // Your insert logic
        return Ok();
    }

    public IActionResult Index()
    {
        var students = _studentService.GetAllStudents();
        return View(students);
    }

    public IActionResult Login() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(Student student)
    {
        bool isValid = _studentService.ValidateStudentLogin(student.Username, student.Password);

        if (isValid)
        {
            HttpContext.Session.SetString("StudentUsername", student.Username);
            return RedirectToAction("Dashboard");
        }

        ViewBag.Message = "Invalid username or password";
        return View();
    }

    public IActionResult Dashboard()
    {
        var username = HttpContext.Session.GetString("StudentUsername");

        if (string.IsNullOrEmpty(username))
            return RedirectToAction("Login");

        var student = _studentService.GetStudentByUsername(username);

        if (student == null)
        {
            ViewBag.Message = "Student not found.";
            return View(new List<Student>());
        }

        ViewBag.Username = username;
        return View(new List<Student> { student });
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
