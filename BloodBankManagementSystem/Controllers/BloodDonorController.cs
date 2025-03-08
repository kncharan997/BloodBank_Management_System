using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BloodBankManagementSystem.Controllers
{
    public class BloodDonorController : Controller
    {
        private readonly BloodBankContext _context;
        public BloodDonorController(BloodBankContext context)
        {
            _context = context;
        }

        // Action to View All Donors
        public IActionResult Index()
        {
            var donors = _context.BloodDonors
                .Include(d => d.Hospital)
                .Include(d => d.BloodDonationCamp)
                .ToList();
            return View(donors);
        }

        // Action to View Details of a Donor
        public IActionResult Details(int id)
        {
            var donor = _context.BloodDonors
                .Include(d => d.Hospital)
                .Include(d => d.BloodDonationCamp)
                .FirstOrDefault(d => d.BloodDonorID == id);

            if (donor == null)
            {
                return NotFound();
            }
            return View(donor);
        }

        // Action to Add New Donor
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BloodDonor donor)
        {
            // Validate Age
            if (donor.Age < 18)
            {
                ModelState.AddModelError("Age", "Age must be 18 or older to donate blood.");
            }

            // Validate Weight
            if (donor.Weight < 40)
            {
                ModelState.AddModelError("Weight", "Weight must be greater than 40 to donate blood.");
            }

            // Validate selection of either Hospital or Blood Donation Camp
            if (donor.HospitalID == null && donor.BloodDonationCampID == null)
            {
                ModelState.AddModelError("", "Please select either a Hospital or a Blood Donation Camp.");
            }

            // Prevent donation if donor has a disease
            if (donor.Disease == "Yes")
            {
                ModelState.AddModelError("Disease", "Sorry, you cannot donate blood due to health conditions.");
            }

            if (ModelState.IsValid)
            {
                // Add donor to the database
                donor.LastDonationDate = DateTime.Now;
                _context.BloodDonors.Add(donor);
                _context.SaveChanges();

                // Determine the associated Blood Bank
                int? bloodBankID = null;
                if (donor.HospitalID != null)
                {
                    bloodBankID = _context.Hospitals
                        .Where(h => h.HospitalID == donor.HospitalID)
                        .Select(h => h.BloodBankID)
                        .FirstOrDefault();
                }
                else if (donor.BloodDonationCampID != null)
                {
                    bloodBankID = _context.BloodDonationCamps
                        .Where(c => c.BloodDonationCampID == donor.BloodDonationCampID)
                        .Select(c => c.BloodBankID)
                        .FirstOrDefault();
                }

                // Update Blood Inventory if a valid Blood Bank is found
                if (bloodBankID != null)
                {
                    UpdateBloodInventory(bloodBankID.Value, donor.BloodType, donor.TotalDonations);
                }

                return RedirectToAction(nameof(Index));
            }

            // Reload dropdowns if validation fails
            LoadDropdowns();
            return View(donor);
        }

        // Action to Edit Donor Details
        public IActionResult Edit(int id)
        {
            var donor = _context.BloodDonors.Find(id);
            if (donor == null)
            {
                return NotFound();
            }

            LoadDropdowns();
            return View(donor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BloodDonor donor)
        {
            if (id != donor.BloodDonorID)
            {
                return NotFound();
            }

            // Validate Age
            if (donor.Age < 18)
            {
                ModelState.AddModelError("Age", "Age must be 18 or older to donate blood.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(donor);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();
            return View(donor);
        }

        // Action to Delete Donor
        public IActionResult Delete(int id)
        {
            var donor = _context.BloodDonors.Find(id);
            if (donor == null)
            {
                return NotFound();
            }
            return View(donor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var donor = _context.BloodDonors.Find(id);
            if (donor == null)
            {
                return NotFound();
            }
            _context.BloodDonors.Remove(donor);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Action to Register a Blood Donation and Update Inventory
        public IActionResult DonateBlood(int id)
        {
            var donor = _context.BloodDonors.Find(id);
            if (donor == null)
            {
                return NotFound();
            }

            int? bloodBankID = donor.HospitalID != null
                ? _context.Hospitals.Where(h => h.HospitalID == donor.HospitalID).Select(h => h.BloodBankID).FirstOrDefault()
                : _context.BloodDonationCamps.Where(c => c.BloodDonationCampID == donor.BloodDonationCampID).Select(c => c.BloodBankID).FirstOrDefault();

            if (bloodBankID == null)
            {
                return BadRequest("No associated Blood Bank found.");
            }

            var bloodInventory = _context.BloodInventories
                .FirstOrDefault(b => b.BloodBankID == bloodBankID && b.BloodGroup == donor.BloodType);

            if (bloodInventory == null || bloodInventory.NumberofBottles <= 0)
            {
                return BadRequest("No sufficient blood bottles available.");
            }

            bloodInventory.NumberofBottles--;
            donor.LastDonationDate = DateTime.Now;
            donor.TotalDonations++;

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Helper Method to Update Blood Inventory
        private void UpdateBloodInventory(int bloodBankID, string bloodGroup, int donatedBottles)
        {
            var inventory = _context.BloodInventories
                .FirstOrDefault(i => i.BloodBankID == bloodBankID && i.BloodGroup == bloodGroup);

            if (inventory != null)
            {
                inventory.NumberofBottles += donatedBottles;
            }
            else
            {
                _context.BloodInventories.Add(new BloodInventory
                {
                    BloodBankID = bloodBankID,
                    BloodGroup = bloodGroup,
                    NumberofBottles = donatedBottles,
                    ExpiryDate = DateTime.Now.AddMonths(1)
                });
            }
            _context.SaveChanges();
        }

        // Helper Method to Load Dropdown Data
        private void LoadDropdowns()
        {
            ViewBag.Hospitals = new SelectList(_context.Hospitals, "HospitalID", "Name");
            ViewBag.Camps = new SelectList(_context.BloodDonationCamps
                .Where(c => c.CampEndDate >= DateTime.Today)  // Only active camps
                .Select(c => new { c.BloodDonationCampID, c.CampName }),
                "BloodDonationCampID", "CampName");
        }
    }
}