using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SosyalMedyaMVC.Models;
using SosyalMedyaMVC.ViewModels;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var userName = HttpContext.Session.GetString("UserName");
        var fullName = HttpContext.Session.GetString("FullName");

        ViewBag.WelcomeMessage = userName != null && fullName != null
            ? $"Hoþgeldiniz, {fullName}!"
            : "Hoþgeldiniz!";

        ViewBag.IsLoggedIn = userName != null && fullName != null;

        var posts = _context.Posts.OrderByDescending(p => p.CreatedDate).ToList();
        return View(posts);
    }

    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("UserName") != null)
        {
            return RedirectToAction("Index");
        }

        ViewBag.IsLoggedIn = false;
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("FullName", $"{user.Name} {user.SurName}");

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Geçersiz giriþ denemesi.");
            }
        }

        ViewBag.IsLoggedIn = false;
        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UserName");
        HttpContext.Session.Remove("FullName");
        return RedirectToAction("Login");
    }

    public IActionResult Explore()
    {
        return View();
    }
}
