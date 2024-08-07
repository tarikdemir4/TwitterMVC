using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SosyalMedyaMVC.Models;
using Microsoft.AspNetCore.Http;
using System.IO;

public class PostController : Controller
{
    private readonly ApplicationDbContext _context;

    public PostController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        var userName = HttpContext.Session.GetString("UserName");
        var fullName = HttpContext.Session.GetString("FullName");

        var post = new Post
        {
            UserName = userName,
            FullName = fullName
        };

        return View(post);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Post post, IFormFile? ImageFile)
    {
        if (ModelState.IsValid)
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }

                    post.ImagePath = $"img/{fileName}";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Resim kaydedilemedi: " + ex.Message);
                    return View(post);
                }
            }

            post.CreatedDate = DateTime.Now;
            post.UserName = HttpContext.Session.GetString("UserName");
            post.FullName = HttpContext.Session.GetString("FullName");

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        return View(post);
    }

    [HttpPost]
    public async Task<IActionResult> LikePost(int postId)
    {
        var userName = HttpContext.Session.GetString("UserName");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
        {
            return RedirectToAction("Login", "Home");
        }

        var userId = user.Id;

        var existingLike = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

        if (existingLike != null)
        {
            TempData["LikeMessage"] = "Bu gönderiyi zaten beğendiniz.";
        }
        else
        {
            var like = new Like
            {
                PostId = postId,
                UserId = userId
            };

            _context.Likes.Add(like);

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
            if (post != null)
            {
                post.LikeCount++;
                await _context.SaveChangesAsync();
            }

            TempData["LikeMessage"] = "Gönderiyi beğendiniz.";
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Details(int id)
    {
        var post = await _context.Posts
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(int postId, string commentText)
    {
        var userName = HttpContext.Session.GetString("UserName");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (user == null)
        {
            return RedirectToAction("Login", "Home");
        }

        var comment = new Comment
        {
            PostId = postId,
            UserId = user.Id,
            Content = commentText,
            CreatedDate = DateTime.Now
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", new { id = postId });
    }
}
