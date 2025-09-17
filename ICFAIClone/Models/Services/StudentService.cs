using ICFAIClone.db;
using ICFAIClone.Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ICFAIClone.Services
{
    public class StudentService
    {
        private readonly ApplicationDbContext _context;
        private readonly SqlConnectionHelper _dbHelper;

        public StudentService(ApplicationDbContext context, SqlConnectionHelper dbHelper)
        {
            _context = context;
            _dbHelper = dbHelper;
        }

        public void InsertStudent(Student student)
        {
            using (var conn = new SqlConnection(_dbHelper.GetConnection().ConnectionString))
            using (var cmd = new SqlCommand("InsertStudent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // ✅ Handle ProfileImage safely
                cmd.Parameters.AddWithValue("@ProfileImage", student.ProfileImage ?? (object)DBNull.Value);

                // Add all required parameters
                cmd.Parameters.AddWithValue("@FullName", student.FullName);
                cmd.Parameters.AddWithValue("@EnrollmentNumber", student.EnrollmentNumber);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                cmd.Parameters.AddWithValue("@Course", student.Course);
                cmd.Parameters.AddWithValue("@Fees", student.Fees);
                cmd.Parameters.AddWithValue("@IsActive", student.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public bool ValidateStudentLogin(string username, string password)
        {
            using (var connection = new SqlConnection(_dbHelper.GetConnection().ConnectionString))
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

        public List<Student> GetAllStudents()
        {
            return _context.Students.ToList();
        }

        public Student GetStudentByUsername(string username)
        {
            return _context.Students.FirstOrDefault(s => s.Username == username);
        }
    }
}
