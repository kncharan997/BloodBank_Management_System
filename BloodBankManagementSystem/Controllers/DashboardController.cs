using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BloodBankManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly BloodBankContext _context;

        public DashboardController(BloodBankContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetStats()
        {
            var stats = new
            {
                totalDonors = _context.BloodDonors.Count(),
                bloodUnits = _context.BloodInventories.Sum(b => b.NumberofBottles),
                donationCamps = _context.BloodDonationCamps.Count()
            };
            return Json(stats);
        }

        [HttpGet]
        public IActionResult GetBloodInventory()
        {
            var inventory = _context.BloodInventories
                .Where(b => b.NumberofBottles > 0) // Exclude null or zero values
                .Select(b => new
                {
                    bloodGroup = b.BloodGroup,
                    numberOfBottles = b.NumberofBottles
                })
                .ToList();

            return Json(inventory);
        }
    }
}