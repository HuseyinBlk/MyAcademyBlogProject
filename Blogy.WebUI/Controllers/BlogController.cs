using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers;

public class BlogController(IBlogService blogService, ICategoryService categoryService) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetBlogsByCategory(int id)
    {
        var blogs = await blogService.GetBlogsByCategoryIdAsync(id);
        var category = await categoryService.GetByIdAsync(id);
        ViewBag.categoryName = category.Name;
        return View(blogs);
    }
    
}