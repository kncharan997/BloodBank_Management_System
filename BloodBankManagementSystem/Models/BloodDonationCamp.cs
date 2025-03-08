using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBankManagementSystem.Models
{
    public class BloodDonationCamp
    {
        public int BloodDonationCampID { get; set; }
        public string? CampName { get; set; }
        public string? Address { get; set; }

        [ForeignKey("BloodBank")]
        public int? BloodBankID { get; set; }

        public virtual BloodBank? BloodBank { get; set; }
        //public int? HospitalID { get; set; }

        //public virtual Hospital? Hospital { get; set; }

        public string? City { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [CustomValidation(typeof(BloodDonationCamp), nameof(ValidateCampStartDate))] //Change here as ValidateCampStartDate
        public DateTime CampStartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        [CustomValidation(typeof(BloodDonationCamp), nameof(ValidateCampEndDate))] //Change here as ValidateCampStartDate
        public DateTime CampEndDate { get; set; }


        public static ValidationResult ValidateCampStartDate(DateTime campstartdate, ValidationContext context)
        {
            if (campstartdate <= DateTime.Today)
            {
                return new ValidationResult("Start date must be beyond today.");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateCampEndDate(DateTime campenddate, ValidationContext context)
        {
            var instance = (BloodDonationCamp)context.ObjectInstance;
            if (campenddate < instance.CampStartDate)
            {
                return new ValidationResult("End date must be beyond start today.");
            }
            return ValidationResult.Success;
        }
        public virtual ICollection<BloodDonor> BloodDonors { get; set; } = new List<BloodDonor>();
    }
}