using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodBankManagementSystem.Controllers
{
    public class BloodBankController : Controller
    {
        private readonly BloodBankContext _context;
        public BloodBankController(BloodBankContext context)
        {
            _context = context;
        }
        // GET: BloodBank
        public IActionResult Index()
        {
            var bloodBanks = _context.BloodBanks.ToList();
            return View(bloodBanks);
        }
        // GET: BloodBank/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bloodBank = _context.BloodBanks.FirstOrDefault(b => b.BloodBankID == id);
            if (bloodBank == null)
            {
                return NotFound();
            }
            return View(bloodBank);
        }

        // GET: BloodBank/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BloodBank/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BloodBank bloodBank)
        {
            if (ModelState.IsValid)
            {
                _context.BloodBanks.Add(bloodBank);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(bloodBank);
        }

        // GET: BloodBank/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodBank = _context.BloodBanks.Find(id);
            if (bloodBank == null)
            {
                return NotFound();
            }

            return View(bloodBank);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, BloodBank bloodBank)
        {
            if (id != bloodBank.BloodBankID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bloodBank);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.BloodBanks.Any(e => e.BloodBankID == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bloodBank);
        }
        // GET: BloodBank/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bloodBank = _context.BloodBanks.FirstOrDefault(b => b.BloodBankID == id);
            if (bloodBank == null)
            {
                return NotFound();
            }

            return View(bloodBank);
        }

        // POST: BloodBank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var bloodBank = _context.BloodBanks.Find(id);
            if (bloodBank != null)
            {
                _context.BloodBanks.Remove(bloodBank);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
