using System.ComponentModel.DataAnnotations;

namespace BloodBankManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "UserName is Mandatory.")]

        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is also Mandatory")]

        [MinLength(6, ErrorMessage = "Password must be atleast 6 characters long.")]

        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&)]+$", ErrorMessage = "Password must contain atleast one uppercase letter,one number,and one special character")]

        public string Password { get; set; }
    }
}
