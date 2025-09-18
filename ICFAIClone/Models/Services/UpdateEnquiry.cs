using ICFAIClone.db;
using ICFAIClone.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ICFAIClone.Services
{
    public class EnquiryService
    {
        private readonly string _connectionString;
        private readonly SqlConnectionHelper _dbHelper;

        public EnquiryService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void UpdateEnquiry(Enquiry enquiry)
        {
            using (var conn = new SqlConnection(_connectionString)) // ✅ Use connection string directly
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Enquiries SET Name = @Name, Email = @Email, Phone = @Phone, Address = @Address WHERE EnquiryId = @EnquiryId", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", enquiry.Name);
                    cmd.Parameters.AddWithValue("@Email", enquiry.Email);
                    cmd.Parameters.AddWithValue("@Phone", enquiry.Phone);
                    cmd.Parameters.AddWithValue("@Address", enquiry.Address);
                    cmd.Parameters.AddWithValue("@EnquiryId", enquiry.EnquiryId); // ✅ Use ID

                    cmd.ExecuteNonQuery();
                }
            }
        }
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

    }
}
