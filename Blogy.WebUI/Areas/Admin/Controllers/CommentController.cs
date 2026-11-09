using Blogy.Business.Dtos.CommentDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogy.WebUI.Areas.Admin.Controllers;

[Authorize(Roles = Roles.Admin)]
[Area(Roles.Admin)]
public class CommentController(ICommentService commentService, IBlogService blogService, UserManager<AppUser> userManager) : Controller
{
    private async Task GetBlogs()
    {
        var blogs = await blogService.GetAllAsync();
        ViewBag.Blogs = (from blog in blogs
                select new SelectListItem
                {
                    Text = blog.Title,
                    Value = blog.Id.ToString()
                });
    }

    public async Task<IActionResult> Index()
    {
        var comments = await commentService.GetCommentsWithBlogAndUserAsync();
        return View(comments);
    }

    public async Task<IActionResult> Create()
    {
        await GetBlogs();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCommentDto dto)
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);
        dto.UserId = user.Id;
        await commentService.CreateAsync(dto);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        await GetBlogs();
        var comment = await commentService.GetByIdAsync(id);
        return View(comment);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateCommentDto dto)
    {
        await commentService.UpdateAsync(dto);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await commentService.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        await commentService.ToggleCommentStatusAsync(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ReanalyzeWithHuggingFace(int id)
    {
        var success = await commentService.ReanalyzeWithHuggingFaceAsync(id);
        if (success)
        {
            TempData["SuccessMessage"] = "Yorum Hugging Face modelleriyle başarıyla analiz edildi!";
        }
        else
        {
            TempData["ErrorMessage"] = "Hugging Face servisi yanıt vermedi (Yetki/Ödeme gerekiyor).";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ReanalyzeWithGemini(int id)
    {
        var success = await commentService.ReanalyzeWithGeminiAsync(id);
        if (success)
        {
            TempData["SuccessMessage"] = "Yorum Google Gemini AI ile başarıyla analiz edildi!";
        }
        else
        {
            TempData["ErrorMessage"] = "Gemini servisi ile analiz yapılırken bir hata oluştu.";
        }
        return RedirectToAction("Index");
    }

}
