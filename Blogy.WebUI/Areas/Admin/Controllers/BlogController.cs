using AutoMapper;
using Blogy.Business.Dtos.BlogDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Entity.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class BlogController(IBlogService blogService) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var blogs = await blogService.GetAllBlogsWithCategoriesAsync();
        return View(blogs);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBlogDto blog)
    {
        if (!ModelState.IsValid)
        {
            return View(blog);
        }
        
        await blogService.CreateAsync(blog);
        return RedirectToAction("Index");
    }
}