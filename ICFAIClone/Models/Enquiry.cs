using ICFAIClone.Models.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace ICFAIClone.Models
{
    
    
public class Enquiry
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? Phone { get; set; }
        public required string Address { get; set; }

        public bool SaveToDatabase(string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("InsertICFAIClone", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", string.IsNullOrEmpty(Name) ? DBNull.Value : Name);
                    cmd.Parameters.AddWithValue("@EMAIL", string.IsNullOrEmpty(Email) ? DBNull.Value : Email);
                    cmd.Parameters.AddWithValue("@PHNo", string.IsNullOrEmpty(Phone)? DBNull.Value: Phone);
                    cmd.Parameters.AddWithValue("@Address", string.IsNullOrEmpty(Address) ? DBNull.Value : Address);


                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
    

}
