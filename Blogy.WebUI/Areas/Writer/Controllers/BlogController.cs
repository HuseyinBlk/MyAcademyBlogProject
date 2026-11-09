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

namespace Blogy.WebUI.Areas.Writer.Controllers;

[Area("Writer")]
[Authorize(Roles = Roles.Writer)]
public class BlogController(IBlogService blogService,ICategoryService categoryService, UserManager<AppUser> userManager, IFileService fileService) : Controller
{
    private async Task GetAllCategories()
    {
        var categories = await categoryService.GetAllAsync();
        ViewBag.Categories = categories.Select(c => new SelectListItem
        {
            Text = c.Name,
            Value = c.Id.ToString()
        }).ToList();
    }

    public async Task<IActionResult> Index()
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);
        if(user == null) return RedirectToAction("Index", "Login");

        var blogs = await blogService.GetBlogByWriterIdAsync(user.Id);
        return View(blogs);
    }

    public async Task<IActionResult> Create()
    {
        await GetAllCategories();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBlogDto blog, IFormFile? coverImageFile)
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);
        if (user == null) return RedirectToAction("Index","Login");

        blog.WriterId = user.Id;

        if (blog.CoverImage != null)
        {
            blog.CoverImage = await fileService.UploadFileAsync(coverImageFile, "image/blogs");
        }

        await blogService.CreateAsync(blog);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await blogService.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (user == null) return RedirectToAction("Index", "Login");

        var blogDetails = await blogService.GetBlogDetailsByIdAsync(id);
        if (blogDetails == null) return NotFound();

        if (blogDetails.WriterId != user.Id)
        {
            return Forbid();
        }

        await GetAllCategories();
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
        var user = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (user == null) return RedirectToAction("Index", "Login");

        var blogDetails = await blogService.GetBlogDetailsByIdAsync(blog.Id);
        if (blogDetails == null || blogDetails.WriterId != user.Id)
        {
            return Forbid();
        }

        if (!ModelState.IsValid)
        {
            await GetAllCategories();
            return View(blog);
        }

        var existingBlog = await blogService.GetByIdAsync(blog.Id);

        if (coverImageFile != null)
        {
            if (!string.IsNullOrEmpty(existingBlog?.CoverImage))
            {
                fileService.DeleteFile(existingBlog.CoverImage);
            }
            blog.CoverImage = await fileService.UploadFileAsync(coverImageFile, "images/blogs");
        }
        else
        {
            blog.CoverImage = existingBlog?.CoverImage;
        }

        if (blogImageFile1 != null)
        {
            if (!string.IsNullOrEmpty(existingBlog?.BlogImage))
            {
                fileService.DeleteFile(existingBlog.BlogImage);
            }
            blog.BlogImage = await fileService.UploadFileAsync(blogImageFile1, "images/blogs");
        }
        else
        {
            blog.BlogImage = existingBlog?.BlogImage;
        }

        if (blogImageFile2 != null)
        {
            if (!string.IsNullOrEmpty(existingBlog?.BlogImage2))
            {
                fileService.DeleteFile(existingBlog.BlogImage2);
            }
            blog.BlogImage2 = await fileService.UploadFileAsync(blogImageFile2, "images/blogs");
        }
        else
        {
            blog.BlogImage2 = existingBlog?.BlogImage2;
        }

        await blogService.UpdateAsync(blog);
        TempData["SuccessMessage"] = "Blog yazınız başarıyla güncellendi!";
        return RedirectToAction("Index");
    }
}
