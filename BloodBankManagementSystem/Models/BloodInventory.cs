using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBankManagementSystem.Models
{
    public class BloodInventory
    {
        [Key]
        public int BloodInventoryID { get; set; }

        [Required]
        public string? BloodGroup { get; set; }

        [Required]
        public int NumberofBottles { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [ForeignKey("BloodBank")]
        public int BloodBankID { get; set; }

        public virtual BloodBank? BloodBank { get; set; }

    }
}