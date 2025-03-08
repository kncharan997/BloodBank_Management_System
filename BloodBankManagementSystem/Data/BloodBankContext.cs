using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace BloodBankManagementSystem.Models
{
    public class BloodBankContext : DbContext
    {
        public BloodBankContext(DbContextOptions<BloodBankContext> options)
        : base(options)
        { }

        public DbSet<BloodDonationCamp> BloodDonationCamps { get; set; }
        public virtual DbSet<BloodDonor> BloodDonors { get; set; }
        public DbSet<BloodBank> BloodBanks { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<BloodInventory> BloodInventories { get; set; }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relationship: One Hospital has many Donors
            modelBuilder.Entity<BloodDonor>()
                .HasOne(d => d.Hospital)
                .WithMany(h => h.BloodDonors)
                .HasForeignKey(d => d.HospitalID)
                .OnDelete(DeleteBehavior.SetNull);

            // Relationship: One Camp has many Donors
            modelBuilder.Entity<BloodDonor>()
                .HasOne(d => d.BloodDonationCamp)
                .WithMany(c => c.BloodDonors)
                .HasForeignKey(d => d.BloodDonationCampID)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}
