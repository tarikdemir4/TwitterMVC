using Microsoft.AspNetCore.Mvc;
using SosyalMedyaMVC.Models;
using SosyalMedyaMVC.ViewModels;

namespace SosyalMedyaMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Register()
        {
            ViewData["BodyClass"] = "register-page";
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.Name,
                    SurName = model.SurName,
                    Email = model.Email,
                    Phone = model.Phone,
                    UserName = model.UserName,
                    Password = model.Password, 
                    Bio = model.Bio
                };

                _context.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult Login()
        {
            ViewData["BodyClass"] = "login-page"; 
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
