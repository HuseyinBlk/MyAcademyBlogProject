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

namespace Blogy.WebUI.Areas.Admin.Controllers;

[Area(Roles.Admin)]
[Authorize(Roles = Roles.Admin)]
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
    public async Task<IActionResult> Generate(string keywords, string summaryPrompt)
    {
        if (string.IsNullOrWhiteSpace(keywords) || string.IsNullOrWhiteSpace(summaryPrompt))
        {
            return Json(new { success = false, message = "Lütfen anahtar kelimeleri ve konu özetini giriniz." });
        }

        try
        {
            var categories = await categoryService.GetAllAsync();
            var categoryNames = categories.Select(c => c.Name).ToList();

            var result = await aiService.GenerateFullBlogAsync(keywords, summaryPrompt, null, categoryNames);

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
        if (coverImageFile != null)
        {
            model.CoverImage = await fileService.UploadFileAsync(coverImageFile, "images/blogs");
        }
        else
        {

            model.CoverImage = "/blogy-1.0.0/images/hero_1.jpg";
        }

        var user = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (user != null)
        {
            model.WriterId = user.Id;
        }

        await blogService.CreateAsync(model);

        TempData["SuccessMessage"] = "Yapay zeka tarafından hazırlanan blog yazınız başarıyla yayınlandı!";
        return RedirectToAction("Index", "Blog", new { area = "Admin" });
    }
}
