using ICFAIClone.Models;
using ICFAIClone.db;
using Microsoft.Data.SqlClient;

namespace ICFAIClone.Services
{
    public class AdminService
    {
        private readonly SqlConnectionHelper _dbHelper;

        public AdminService(SqlConnectionHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<Enquiry> GetAllEnquiries()
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
    }
}
