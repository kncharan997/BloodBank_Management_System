using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBankManagementSystem.Models
{
    public class Hospital
    {
        public int HospitalID { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Contact { get; set; }
        public string? Address { get; set; }
        [ForeignKey("BloodBank")]
        public int? BloodBankID { get; set; }

        public virtual BloodBank? BloodBank { get; set; }
        public virtual ICollection<BloodDonor> BloodDonors { get; set; } = new List<BloodDonor>();
    }
}
