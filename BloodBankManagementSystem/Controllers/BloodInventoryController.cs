using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodBankManagementSystem.Controllers
{
    public class BloodInventoryController : Controller
    {
        private readonly BloodBankContext _context;
        public BloodInventoryController(BloodBankContext context)
        {
            _context = context;
        }
        public IActionResult RemoveExpiredBlood()
        {
            // Get all expired blood records
            var expiredBlood = _context.BloodInventories
                .Where(b => b.ExpiryDate < DateTime.Today)
                .ToList();

            if (expiredBlood.Any())
            {
                _context.BloodInventories.RemoveRange(expiredBlood);
                _context.SaveChanges();
            }

            return RedirectToAction("Index"); // Redirect to inventory list
        }
        // View Blood Inventory
        public IActionResult Index()
        {
            var inventory = _context.BloodInventories.Include(b => b.BloodBank).ToList();
            return View(inventory);
        }

        // Add Blood Stock - GET
        public IActionResult Create()
        {
            ViewBag.BloodBanks = _context.BloodBanks.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BloodInventory bloodInventory)
        {
            if (ModelState.IsValid)
            {
                var bloodBank = _context.BloodBanks.FirstOrDefault(b => b.BloodBankID == bloodInventory.BloodBankID);
                if (bloodBank == null)
                {
                    ModelState.AddModelError("", "Invalid Blood Bank.");
                    ViewBag.BloodBanks = _context.BloodBanks.ToList();
                    return View(bloodInventory);
                }

                bloodInventory.BloodBank = bloodBank;
                _context.BloodInventories.Add(bloodInventory);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.BloodBanks = _context.BloodBanks.ToList();
            return View(bloodInventory);
        }

        public IActionResult Transfer(int id)
        {
            var blood = _context.BloodInventories.Include(b => b.BloodBank).FirstOrDefault(b => b.BloodInventoryID == id);
            if (blood == null) return NotFound();

            ViewBag.BloodBanks = _context.BloodBanks
                                        .Where(b => b.BloodBankID != blood.BloodBankID) // Exclude current blood bank
                                        .ToList();
            return View(blood);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Transfer(int id, int quantity, int targetBloodBankId)
        {
            var blood = _context.BloodInventories.Include(b => b.BloodBank).FirstOrDefault(b => b.BloodInventoryID == id);
            var targetBloodBank = _context.BloodBanks.FirstOrDefault(b => b.BloodBankID == targetBloodBankId);

            if (blood == null || targetBloodBank == null)
                return NotFound();

            // Prevent transferring to the same blood bank
            if (blood.BloodBankID == targetBloodBank.BloodBankID)
            {
                ModelState.AddModelError("", "Cannot transfer to the same Blood Bank.");
                ViewBag.BloodBanks = _context.BloodBanks.Where(b => b.BloodBankID != blood.BloodBankID).ToList(); // Exclude current bank
                return View(blood);
            }

            // Ensure transfer quantity is valid
            if (quantity < 1 || quantity > blood.NumberofBottles)
                return BadRequest("Invalid transfer quantity.");

            // Subtract from source
            blood.NumberofBottles -= quantity;

            // Remove the entry if bottles become zero
            if (blood.NumberofBottles == 0)
            {
                _context.BloodInventories.Remove(blood);
            }

            // Check if the target blood bank already has this blood group
            var targetBloodInventory = _context.BloodInventories
                                               .FirstOrDefault(b => b.BloodBankID == targetBloodBank.BloodBankID && b.BloodGroup == blood.BloodGroup);

            if (targetBloodInventory == null)
            {
                // If not found, create a new record
                targetBloodInventory = new BloodInventory
                {
                    BloodGroup = blood.BloodGroup,
                    NumberofBottles = quantity,
                    ExpiryDate = blood.ExpiryDate,
                    BloodBankID = targetBloodBank.BloodBankID
                };
                _context.BloodInventories.Add(targetBloodInventory);
            }
            else
            {
                // Update existing record
                targetBloodInventory.NumberofBottles += quantity;
            }

            // Save changes
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Action to Delete Donor
        public IActionResult Delete(int id)
        {
            var donor = _context.BloodInventories.FirstOrDefault(d => d.BloodInventoryID == id);
            if (donor == null)
            {
                return NotFound();
            }
            return View(donor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var donor = _context.BloodInventories.FirstOrDefault(d => d.BloodInventoryID == id);
            _context.BloodInventories.Remove(donor);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
