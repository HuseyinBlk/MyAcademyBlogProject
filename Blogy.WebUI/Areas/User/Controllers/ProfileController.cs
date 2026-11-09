using AutoMapper;
using Blogy.Business.Dtos.UserDtos;
using Blogy.Business.Services.FileServices;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.User.Controllers;

[Area(Roles.User)]
[Authorize(Roles = Roles.User)]
public class ProfileController(UserManager<AppUser> userManager, IFileService fileService, IMapper mapper) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (user == null) return NotFound();

        var model = mapper.Map<EditProfileDto>(user);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SaveProfile(EditProfileDto model)
    {
        var user = await userManager.FindByNameAsync(User.Identity!.Name!);
        if (user == null) return NotFound();

        mapper.Map(model, user);

        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            if (!string.IsNullOrEmpty(user.ProfilImageUrl))
            {
                fileService.DeleteFile(user.ProfilImageUrl);
            }
            user.ProfilImageUrl = await fileService.UploadFileAsync(model.ImageFile, "images/users/profileImages");
        }

        if (!string.IsNullOrWhiteSpace(model.CurrentPassword) && !string.IsNullOrWhiteSpace(model.NewPassword))
        {
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                ModelState.AddModelError(string.Empty, "Yeni şifreler birbiriyle eşleşmiyor.");
                return View("Index", model);
            }

            var passwordResult = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!passwordResult.Succeeded)
            {
                foreach (var error in passwordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("Index", model);
            }
        }

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("Index", model);
        }

        TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
}
