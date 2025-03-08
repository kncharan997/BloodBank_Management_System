using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodBankManagementSystem.Controllers
{
    public class BloodDonationCampController : Controller
    {
        private readonly BloodBankContext _context;
        public BloodDonationCampController(BloodBankContext context)
        {
            _context = context;
        }
        // GET: BloodDonationCamp/Index
        public IActionResult Index()
        {
            var camps = _context.BloodDonationCamps.ToList();
            return View(camps);
        }

        // GET: BloodDonationCamp/Create
        public IActionResult Create()
        {
            ViewBag.BloodBanks = _context.BloodBanks.ToList(); // Pass the list of blood banks to the view
            return View();
        }

        // POST: BloodDonationCamp/Create
        [HttpPost]
        public IActionResult Create(BloodDonationCamp camp)
        {
            if (camp.CampStartDate <= DateTime.Today)
            {
                ModelState.AddModelError("CampStartDate", "Start date must be beyond toady.");
            }
            if (camp.CampEndDate < camp.CampStartDate)
            {
                ModelState.AddModelError("CampEndDate", "End date must be beyond Start day.");
            }
            if (ModelState.IsValid)
            {
                _context.BloodDonationCamps.Add(camp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BloodBanks = _context.BloodBanks.ToList(); // Re-fetch blood banks if model is invalid
            return View(camp);
        }

        // GET: BloodDonationCamp/Edit/5
        public IActionResult Edit(int id)
        {
            var camp = _context.BloodDonationCamps.FirstOrDefault(c => c.BloodDonationCampID == id);
            if (camp == null || camp.CampStartDate < DateTime.Now)
            {
                // If camp is not found or start date is past, deny access to edit
                return NotFound();
            }

            return View(camp);
        }

        // POST: BloodDonationCamp/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, BloodDonationCamp camp)
        {
            if (id != camp.BloodDonationCampID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(camp);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.BloodDonationCamps.Any(c => c.BloodDonationCampID == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(camp);
        }

        // GET: BloodDonationCamp/Details/5
        public IActionResult Details(int id)
        {
            var camp = _context.BloodDonationCamps.FirstOrDefault(c => c.BloodDonationCampID == id);
            if (camp == null)
            {
                return NotFound();
            }
            return View(camp);
        }

        // GET: BloodDonationCamp/Delete/5
        public IActionResult Delete(int id)
        {
            var camp = _context.BloodDonationCamps.FirstOrDefault(c => c.BloodDonationCampID == id);
            if (camp == null)
            {
                return NotFound();
            }
            return View(camp);
        }

        // POST: BloodDonationCamp/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var camp = _context.BloodDonationCamps.FirstOrDefault(c => c.BloodDonationCampID == id);
            if (camp != null)
            {
                _context.BloodDonationCamps.Remove(camp);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
