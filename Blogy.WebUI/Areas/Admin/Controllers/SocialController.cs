using Blogy.Business.Dtos.SocialDtos;
using Blogy.Business.Services.SocialServices;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers;

[Area(Roles.Admin)]
[Authorize(Roles = Roles.Admin)]

public class SocialController(ISocialService socialService) : Controller
{

    public async Task<IActionResult> Index()
    {
        var socials = await socialService.GetAllAsync();
        return View(socials);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSocialDto dto)
    {
        await  socialService.CreateAsync(dto);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var social = await socialService.GetByIdAsync(id);
        return View(social);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateSocialDto dto)
    {
        await socialService.UpdateAsync(dto);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await socialService.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}
