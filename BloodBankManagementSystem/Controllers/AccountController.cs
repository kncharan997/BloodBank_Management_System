using BloodBankManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
public class AccountController : Controller
{
    private readonly BloodBankContext _context;

    public AccountController(BloodBankContext context)
    {
        _context = context;
    }

    // GET: Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // POST: Login
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        // Query the database to find the user with the provided username and password
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);

        if (user != null)
        {
            // Store the user in session on successful login
            HttpContext.Session.SetInt32("UserID", user.UserID);
            HttpContext.Session.SetString("UserName", user.UserName);

            // Redirect to the homepage after successful login
            return RedirectToAction("Index", "Home");
        }

        else
        {
            // If login fails, show an error message

            ViewBag.ErrorMessage = "Invalid username or password.";

            return View();
        }
    }

    // Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear session data
        return RedirectToAction("Login");
    }

    //register
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            if (_context.Users.Any(u => u.UserName == user.UserName))
            {
                ModelState.AddModelError("UserName", "UserName already exists");
                return View(user);
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }
        return View(user);
    }
}

