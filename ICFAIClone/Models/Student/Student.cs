using System.ComponentModel.DataAnnotations;

namespace ICFAIClone.Models.Entities
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string EnrollmentNumber { get; set; }
        public required string Course { get; set; }
        public DateOnly? DateOfBirth { get; set; } // ✅ Correct type
        public required long Fees { get; set; }
        public bool IsActive { get; set; }
        public string? ProfileImage { get; set; }




        [Required]
        [Display(Name = "Username")]
        public required string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }


}
