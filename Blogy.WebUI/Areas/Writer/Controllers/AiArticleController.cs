using Blogy.Business.Dtos.BlogDtos;
using Blogy.Business.Services.AiServices;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.FileServices;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogy.WebUI.Areas.Writer.Controllers;

[Area(Roles.Writer)]
[Authorize(Roles = Roles.Writer)]
public class AiArticleController(
    IAiService aiService,
    ICategoryService categoryService,
    IBlogService blogService,
    IFileService fileService,
    UserManager<AppUser> userManager) : Controller
{
    private async Task PopulateCategoriesAsync()
    {
        var categories = await categoryService.GetAllAsync();
        ViewBag.Categories = categories.Select(c => new SelectListItem
        {
            Text = c.Name,
            Value = c.Id.ToString()
        }).ToList();
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        await PopulateCategoriesAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Generate(string? keywords, string? rawDraft, string? customInstructions)
    {
        if (string.IsNullOrWhiteSpace(keywords) && string.IsNullOrWhiteSpace(rawDraft))
        {
            return Json(new { success = false, message = "Lütfen en azından anahtar kelimeleri veya geliştirmek istediğiniz taslak metni giriniz." });
        }

        try
        {
            var categories = await categoryService.GetAllAsync();
            var categoryNames = categories.Select(c => c.Name).ToList();

            var result = await aiService.GenerateFullBlogAsync(keywords, rawDraft, customInstructions, categoryNames);

            return Json(new
            {
                success = true,
                title = result.Title,
                content = result.Content,
                tags = result.Tags,
                suggestedCategory = result.SuggestedCategory
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"Yapay zeka hatası: {ex.Message}" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> PublishBlog(CreateBlogDto model, IFormFile? coverImageFile)
    {
        var user = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (user == null) return RedirectToAction("Index", "Login");

        model.WriterId = user.Id;

        if (coverImageFile != null)
        {
            model.CoverImage = await fileService.UploadFileAsync(coverImageFile, "images/blogs");
        }
        else
        {
            model.CoverImage = "/blogy-1.0.0/images/hero_1.jpg";
        }

        await blogService.CreateAsync(model);

        TempData["SuccessMessage"] = "AI destekli blog yazınız başarıyla yayınlandı!";
        return RedirectToAction("Index", "Blog", new { area = "Writer" });
    }
}
