using Microsoft.AspNetCore.Mvc;

namespace BloodBankManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private readonly ISession session;
        public HomeController(IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext.Session;
        }
        public IActionResult Index()
        {
            // Check if user is logged in (i.e., UserID is stored in session)
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                // Redirect to login page if the user is not logged in
                return RedirectToAction("Login", "Account");
            }

            // Return home page if user is logged in
            return View();
        }

        // Search functionality (optional, based on your needs)
        [HttpGet]
        public IActionResult Search(string query)
        {
            ViewData["SearchResult"] = query;
            return View("Index"); // Redirect back to Index after search
        }
    }
}
