using ICFAIClone.db;
using ICFAIClone.Models;
using ICFAIClone.Models.Entities;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace ICFAIClone.Services
{
    public class AdminService
    {
        private readonly SqlConnectionHelper _dbHelper;

        public AdminService(SqlConnectionHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // ✅ Add this method
        public Enquiry GetEnquiryById(int enquiryId)
        {
            Enquiry enquiry = null;

            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT EnquiryId, Name, Email, Phone, Address FROM Enquiries WHERE EnquiryId = @EnquiryId", conn))
                {
                    cmd.Parameters.AddWithValue("@EnquiryId", enquiryId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            enquiry = new Enquiry
                            {
                                EnquiryId = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Email = reader.GetString(2),
                                Phone = reader.GetString(3),
                                Address = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return enquiry;
        }

        // ✅ Make sure this also exists
        public void UpdateEnquiry(Enquiry enquiry)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(
                    "UPDATE Enquiries SET Name = @Name, Email = @Email, Phone = @Phone, Address = @Address WHERE EnquiryId = @EnquiryId", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", enquiry.Name);
                    cmd.Parameters.AddWithValue("@Email", enquiry.Email);
                    cmd.Parameters.AddWithValue("@Phone", enquiry.Phone);
                    cmd.Parameters.AddWithValue("@Address", enquiry.Address);
                    cmd.Parameters.AddWithValue("@EnquiryId", enquiry.EnquiryId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Enquiry> GetAllEnquiries()
        {
            var list = new List<Enquiry>();

            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT EnquiryId, Name, Email, Phone, Address FROM Enquiries", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Enquiry
                        {
                            EnquiryId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Phone = reader.GetString(3),
                            Address = reader.GetString(4)
                        });
                    }
                }
            }

            return list;
        }

        public List<Student> GetAllStudents()
        {
            var list = new List<Student>();

            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Students", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Student
                        {
                            StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            Password = reader.GetString(reader.GetOrdinal("Password")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            Course = reader.GetString(reader.GetOrdinal("Course")),
                            DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DateOfBirth"))),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            EnrollmentNumber = reader.GetString(reader.GetOrdinal("EnrollmentNumber")),
                            FullName = reader.GetString(reader.GetOrdinal("FullName")),
                            Fees = reader.GetInt64(reader.GetOrdinal("Fees")) // or Convert.ToDecimal(...) if needed
                        });

                    }
                }
            }

            return list;
        }

        public Student GetStudentById(int studentId)
        {
            Student student = null;

            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Students WHERE StudentId = @StudentId", conn))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            student = new Student
                            {
                                StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                Course = reader.GetString(reader.GetOrdinal("Course")),
                                DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DateOfBirth"))),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                EnrollmentNumber = reader.GetString(reader.GetOrdinal("EnrollmentNumber")),
                                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                Fees = reader.GetInt64(reader.GetOrdinal("Fees"))
                            };
                        }
                    }
                }
            }

            return student;
        }

        public void UpdateStudent(Student student)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqlCommand(
                    @"UPDATE Students SET Username = @Username, Password = @Password, IsActive = @IsActive, 
              Course = @Course, DateOfBirth = @DateOfBirth, Email = @Email, EnrollmentNumber = @EnrollmentNumber, 
              FullName = @FullName, Fees = @Fees WHERE StudentId = @StudentId", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", student.Username);
                    cmd.Parameters.AddWithValue("@Password", student.Password);
                    cmd.Parameters.AddWithValue("@IsActive", student.IsActive);
                    cmd.Parameters.AddWithValue("@Course", student.Course);
                    cmd.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth.HasValue
                        ? student.DateOfBirth.Value.ToDateTime(TimeOnly.MinValue)
                        : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", student.Email);
                    cmd.Parameters.AddWithValue("@EnrollmentNumber", student.EnrollmentNumber);
                    cmd.Parameters.AddWithValue("@FullName", student.FullName);
                    cmd.Parameters.AddWithValue("@Fees", student.Fees);
                    cmd.Parameters.AddWithValue("@StudentId", student.StudentId);


                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteStudent(int studentId)
        {
            using (SqlConnection conn = _dbHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand("sp_DeleteStudent", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                    conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
