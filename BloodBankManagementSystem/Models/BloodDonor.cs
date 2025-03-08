// Models/BloodDonor.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBankManagementSystem.Models
{
    public class BloodDonor
    {
        public int BloodDonorID { get; set; } // Primary Key
        public string? Name { get; set; }
        public string? BloodType { get; set; }
        public string? Address { get; set; }
        public int HBCount { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Range(40, 100, ErrorMessage = "Weight must be between 40 and 100")]
        public float Weight { get; set; }
        public string? Disease { get; set; }

        [Required(ErrorMessage = "Contact Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact Number must be exactly 10 digits")]
        public string? ContactNumber { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(18, 65, ErrorMessage = "Age must be between 18 and 65")]
        public int? Age { get; set; }

        public DateTime LastDonationDate { get; set; }
        public int TotalDonations { get; set; } // Track number of donations

        [ForeignKey("Hospital")]
        public int? HospitalID { get; set; }

        public virtual Hospital? Hospital { get; set; }

        [ForeignKey("BloodDonationCamp")]
        public int? BloodDonationCampID { get; set; }

        public virtual BloodDonationCamp? BloodDonationCamp { get; set; }
    }
}
