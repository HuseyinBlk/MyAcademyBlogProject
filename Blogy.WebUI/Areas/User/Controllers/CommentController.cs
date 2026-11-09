using Blogy.Business.Services.CommentServices;
using Blogy.DataAccess.Repositories.CommentRepositories;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.User.Controllers;

[Area(Roles.User)]
[Authorize(Roles = Roles.User)]
public class CommentController(ICommentService commentService, UserManager<AppUser> userManager) : Controller
{

    public async Task<IActionResult> Index()
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);
        if (user == null) return NotFound();

        var comments = await commentService.GetCommentsByUserIdAsync(user.Id);
        return View(comments);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await commentService.DeleteAsync(id);
        TempData["SuccessMessage"] = "Yorumunuz başarıyla silindi.";
        return RedirectToAction("Index");
    }
}
