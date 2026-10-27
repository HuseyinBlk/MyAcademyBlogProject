using Blogy.Business.Dtos.UserDtos;
using Blogy.Business.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers;

public class LoginController(IUserService userService) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await userService.LoginAsync(model);
        
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
            return View(model);
        }
        
        return RedirectToAction("Index", "Blog", new { area = "Admin" });
    }

    public async Task<IActionResult> Logout()
    {
        await userService.LogoutAsync();
        return RedirectToAction("Index", "Login");
    }
}