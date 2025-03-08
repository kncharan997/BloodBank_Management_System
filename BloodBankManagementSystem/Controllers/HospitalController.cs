using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankManagementSystem.Controllers
{
    public class HospitalController : Controller
    {
        private readonly BloodBankContext _context;
        public HospitalController(BloodBankContext context)
        {
            _context = context;
        }

        // GET: Hospital
        public IActionResult Index()
        {
            var hospitals = _context.Hospitals.ToList();
            return View(hospitals);
        }

        // GET: Hospital/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var hospital = _context.Hospitals.FirstOrDefault(h => h.HospitalID == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // GET: Hospital/Create
        public IActionResult Create()
        {
            ViewBag.BloodBanks = _context.BloodBanks.ToList();
            return View();
        }

        // POST: Hospital/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hospital hospital)
        {
            if (ModelState.IsValid)
            {
                _context.Hospitals.Add(hospital);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.BloodBanks = _context.BloodBanks.ToList();
            return View(hospital);
        }
        // GET: Hospital/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = _context.Hospitals.Find(id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // POST: Hospital/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Hospital hospital)
        {
            if (id != hospital.HospitalID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(hospital);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(hospital);
        }

        // GET: Hospital/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = _context.Hospitals.FirstOrDefault(h => h.HospitalID == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // POST: Hospital/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hospital = _context.Hospitals.Find(id);
            if (hospital != null)
            {
                _context.Hospitals.Remove(hospital);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
