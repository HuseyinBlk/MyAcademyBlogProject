using AutoMapper;
using Blogy.Business.Dtos.BlogDtos;
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

[Authorize(Roles = Roles.Admin)]
[Area(Roles.Admin)]
public class BlogController(IBlogService blogService, ICategoryService categoryService, IFileService fileService, UserManager<AppUser> userManager) : Controller
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
    public async Task<IActionResult> Create(
        CreateBlogDto model,
        IFormFile? coverImageFile,
        IFormFile? blogImageFile1,
        IFormFile? blogImageFile2)
    {

        if (coverImageFile != null)
        {
            model.CoverImage = await fileService.UploadFileAsync(coverImageFile, "images/blogs");
        }

        if (blogImageFile1 != null)
        {
            model.BlogImage = await fileService.UploadFileAsync(blogImageFile1, "images/blogs");
        }

        if (blogImageFile2 != null)
        {
            model.BlogImage2 = await fileService.UploadFileAsync(blogImageFile2, "images/blogs");
        }

        var user = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (user != null)
        {
            model.WriterId = user.Id;
        }

        await blogService.CreateAsync(model);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        await GetCategoriesAsync();
        var blog = await blogService.GetByIdAsync(id);
        return View(blog);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(
        UpdateBlogDto blog,
        IFormFile? coverImageFile,
        IFormFile? blogImageFile1,
        IFormFile? blogImageFile2)
    {
        if (!ModelState.IsValid)
        {
            await GetCategoriesAsync();
            return View(blog);
        }

        var existingBlog = await blogService.GetByIdAsync(blog.Id);
        if (existingBlog == null) return NotFound();

        if (coverImageFile != null)
        {
            if (!string.IsNullOrEmpty(existingBlog.CoverImage))
            {
                fileService.DeleteFile(existingBlog.CoverImage);
            }
            blog.CoverImage = await fileService.UploadFileAsync(coverImageFile, "images/blogs");
        }
        else
        {
            blog.CoverImage = existingBlog.CoverImage;
        }

        if (blogImageFile1 != null)
        {
            if (!string.IsNullOrEmpty(existingBlog.BlogImage))
            {
                fileService.DeleteFile(existingBlog.BlogImage);
            }
            blog.BlogImage = await fileService.UploadFileAsync(blogImageFile1, "images/blogs");
        }
        else
        {
            blog.BlogImage = existingBlog.BlogImage;
        }

        if (blogImageFile2 != null)
        {
            if (!string.IsNullOrEmpty(existingBlog.BlogImage2))
            {
                fileService.DeleteFile(existingBlog.BlogImage2);
            }
            blog.BlogImage2 = await fileService.UploadFileAsync(blogImageFile2, "images/blogs");
        }
        else
        {
            blog.BlogImage2 = existingBlog.BlogImage2;
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
