using AutoMapper;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

namespace Blogy.WebUI.Controllers;

public class BlogController(IBlogService blogService, ICategoryService categoryService, IMapper mapper) : Controller
{
    // GET
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
    {
        var blogs = await blogService.GetAllAsync();
    
        var values = blogs.AsQueryable().ToPagedList(pageNumber, pageSize);
        return View(values);
    }

    public async Task<IActionResult> GetBlogsByCategory(int id)
    {
        var blogs = await blogService.GetBlogsByCategoryIdAsync(id);
        var category = await categoryService.GetByIdAsync(id);
        ViewBag.categoryName = category.Name;
        return View(blogs);
    }

    public async Task<IActionResult> Details(int id)
    {
        var blog = await blogService.GetBlogDetailsByIdAsync(id);
        return View(blog);
    }
    
}