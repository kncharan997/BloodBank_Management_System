

using System.ComponentModel.DataAnnotations;

namespace BloodBankManagementSystem.Models
{
    public class BloodBank
    {
        public int BloodBankID { get; set; }

        public string? Name { get; set; }

        public string? City { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact Number must be exactly 10 digits")]
        public string? Contact { get; set; }

        // Navigation property for related hospitals
        public virtual ICollection<Hospital> Hospitals { get; set; } = new List<Hospital>();
    }
}
