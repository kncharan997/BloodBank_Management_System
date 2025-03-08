using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

public class SearchController : Controller
{
    private readonly BloodBankContext _context;
    public SearchController(BloodBankContext context)
    {
        _context = context;
    }
    public IActionResult SearchDonors(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return RedirectToAction("Index", "BloodDonor");
        }

        var donors = _context.BloodDonors
                             .Where(d => d.Name.Contains(query) || d.BloodType.Contains(query))
                             .ToList();

        return View(donors);
    }
}