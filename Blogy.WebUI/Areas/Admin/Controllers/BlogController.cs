using AutoMapper;
using Blogy.Business.Dtos.BlogDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blogy.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class BlogController(IBlogService blogService, ICategoryService categoryService) : Controller
{
    private async Task GetCategoriesAsync()
    {
        var categories = await categoryService.GetAllAsync();
        ViewBag.Categories = (from category in categories
                select new SelectListItem
                {
                    Text = category.Name,
                    Value = category.Id.ToString()
                }).ToList();
    }
    
    // GET
    public async Task<IActionResult> Index()
    {
        var blogs = await blogService.GetBlogsWithCategoriesAsync();
        return View(blogs);
    }

    public async Task<IActionResult> Create()
    {
        await GetCategoriesAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBlogDto blog)
    {
        if (!ModelState.IsValid)
        {
            await GetCategoriesAsync();
            return View(blog);
        }
        
        await blogService.CreateAsync(blog);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        await GetCategoriesAsync();
        var blog = await blogService.GetByIdAsync(id);
        return View(blog);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateBlogDto blog)
    {
        if (!ModelState.IsValid)
        {
            await GetCategoriesAsync();
            return View(blog);
        }
        
        await blogService.UpdateAsync(blog);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        await blogService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
    
}