using Blogy.Business.Dtos.UserDtos;
using Blogy.Business.Services.UserServices;
using Blogy.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers;

public class RegisterController(IUserService userService) : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await userService.RegisterAsync(model);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return View(model);
        }
        
        return RedirectToAction("Index", "Login");

    }
}