using Blogy.Business.Services.BlogServices;
using Blogy.Business.Services.CategoryServices;
using Blogy.Business.Services.CommentServices;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Blogy.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogy.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class DashboardController(
    IBlogService blogService,
    ICategoryService categoryService,
    ICommentService commentService,
    UserManager<AppUser> userManager) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var blogs = await blogService.GetBlogsWithCategoriesAsync();
        var categories = await categoryService.GetCategoriesWithBlogsAsync();
        var comments = await commentService.GetCommentsWithBlogAndUserAsync();
        var writers = await userManager.GetUsersInRoleAsync(Roles.Writer);
        var totalUsers = await userManager.Users.CountAsync();

        var viewModel = new AdminDashboardViewModel
        {
            TotalBlogCount = blogs.Count,
            TotalCategoryCount = categories.Count,
            TotalCommentCount = comments.Count,
            ToxicCommentCount = comments.Count(c => c.IsToxic),
            CleanCommentCount = comments.Count(c => !c.IsToxic),
            TotalWriterCount = writers.Count,
            TotalUserCount = totalUsers,

            CategoryLabels = categories.Select(c => c.Name).ToList(),
            CategoryBlogCounts = categories.Select(c => c.Blogs?.Count ?? 0).ToList(),

            RecentBlogs = blogs.OrderByDescending(b => b.Id).Take(5).ToList(),
            RecentComments = comments.OrderByDescending(c => c.Id).Take(5).ToList()
        };

        return View(viewModel);
    }
}
