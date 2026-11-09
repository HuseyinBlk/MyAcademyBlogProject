using AutoMapper;
using Blogy.Business.Dtos.CommentDtos;
using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Business.Services.TagServices;
using Blogy.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

namespace Blogy.WebUI.Controllers;

public class BlogController(
    IBlogService blogService,
    ICommentService commentService,
    ICategoryService categoryService,
    ITagService tagService,
    IMapper mapper,
    UserManager<AppUser> userManager) : Controller
{

    public async Task<IActionResult> Index(string? search = null, int pageNumber = 1, int pageSize = 5)
    {
        var blogs = await blogService.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(search))
        {
            blogs = blogs.Where(x =>
                (x.Title != null && x.Title.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                (x.Description != null && x.Description.Contains(search, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            ViewBag.Search = search;
        }

        var values = blogs.AsQueryable().ToPagedList(pageNumber, pageSize);
        return View(values);
    }

    public async Task<IActionResult> GetBlogsByCategory(int id, int pageNumber = 1, int pageSize = 5)
    {
        var blogs = await blogService.GetBlogsByCategoryIdAsync(id);
        var category = await categoryService.GetByIdAsync(id);
        ViewBag.categoryName = category?.Name ?? "Kategori";
        ViewBag.categoryId = id;

        var pagedBlogs = blogs.AsQueryable().ToPagedList(pageNumber, pageSize);
        return View(pagedBlogs);
    }

    public async Task<IActionResult> Details(int id)
    {
        var blog = await blogService.GetBlogDetailsByIdAsync(id);
        return View(blog);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(CreateCommentDto dto)
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);
        dto.UserId = user.Id;
        await commentService.CreateAsync(dto);
        return RedirectToAction("Details",new {id = dto.BlogId});
    }

    public async Task<IActionResult> GetBlogsByTag(int id, int pageNumber = 1, int pageSize = 5)
    {
        var tag = await tagService.GetByIdAsync(id);
        if (tag == null) return NotFound();

        ViewBag.tagName = tag.Name;
        ViewBag.tagId = id;

        var blogs = await blogService.GetBlogsByTagIdAsync(id);
        var pagedBlogs = blogs.AsQueryable().ToPagedList(pageNumber, pageSize);
        return View(pagedBlogs);
    }

}
