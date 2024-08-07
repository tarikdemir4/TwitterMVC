using Microsoft.AspNetCore.Mvc;
using SosyalMedyaMVC.Models;

public class MessageController : Controller
{
    private readonly ApplicationDbContext _context;

    public MessageController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult SendMessage()
    {
        var currentUser = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(currentUser))
        {
            return RedirectToAction("Home", "Index");
        }

        return View(new SendMessageViewModel());
    }

    [HttpPost]
    public IActionResult SendMessage(SendMessageViewModel model)
    {
        if (ModelState.IsValid)
        {
            ViewData["Error"] = "Form verileri geçersiz!";
            return View(model);
        }

        var receiver = _context.Users.FirstOrDefault(u => u.UserName == model.Receiver);
        if (receiver == null)
        {
            ViewData["Error"] = "Alıcı bulunamadı!";
            return View(model);
        }

        var sender = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(sender))
        {
            ViewData["Error"] = "Oturum bilgisi mevcut değil!";
            return View(model);
        }

        var message = new Message
        {
            Sender = sender,
            Receiver = model.Receiver,
            Content = model.Content,
            SendDate = DateTime.Now
        };

        _context.Messages.Add(message);
        _context.SaveChanges();

        ViewData["Message"] = "Mesaj başarıyla gönderildi!";
        return RedirectToAction("Inbox");
    }

    [HttpGet]
    public IActionResult Inbox()
    {
        var userName = HttpContext.Session.GetString("UserName");
        if (string.IsNullOrEmpty(userName))
        {
            return RedirectToAction("Login", "Home");
        }

        var messages = _context.Messages
            .Where(m => m.Receiver == userName)
            .Select(m => new MessageViewModel
            {
                Sender = m.Sender,
                Content = m.Content,
                SendDate = m.SendDate
            })
            .ToList();

        return View(messages);
    }
}
