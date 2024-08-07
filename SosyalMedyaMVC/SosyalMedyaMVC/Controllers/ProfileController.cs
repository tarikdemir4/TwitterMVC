using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SosyalMedyaMVC.Models;
using System.Linq;
using System.Threading.Tasks;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProfileController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var userName = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(userName))
        {
            return RedirectToAction("Login", "Home");
        }

        var user = _context.Users
            .Where(u => u.UserName == userName)
            .FirstOrDefault();

        if (user == null)
        {
            return RedirectToAction("Login", "Home");
        }

        return View(user);
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login", "Home");
    }

    [HttpGet]
    public IActionResult Update()
    {
        var userName = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(userName))
        {
            return RedirectToAction("Login", "Home");
        }

        var user = _context.Users
            .Where(u => u.UserName == userName)
            .FirstOrDefault();

        if (user == null)
        {
            return RedirectToAction("Login", "Home");
        }

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Update(User updatedUser)
    {
        if (!ModelState.IsValid)
        {
            return View(updatedUser);
        }

        var user = _context.Users
            .Where(u => u.UserName == updatedUser.UserName)
            .FirstOrDefault();

        if (user != null)
        {
            user.Name = updatedUser.Name;
            user.SurName = updatedUser.SurName;
            user.Email = updatedUser.Email;
            user.Phone = updatedUser.Phone;
            user.Bio = updatedUser.Bio;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Kullanıcı bulunamazsa hata gösterebilirsiniz
        ModelState.AddModelError("", "Kullanıcı bulunamadı.");
        return View(updatedUser);
    }
}
