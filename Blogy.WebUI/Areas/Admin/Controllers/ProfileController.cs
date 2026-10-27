using AutoMapper;
using Blogy.Business.Dtos.UserDtos;
using Blogy.Business.Services.FileServices;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area(Roles.Admin)]
public class ProfileController(UserManager<AppUser> userManager, IMapper mapper, IFileService fileService) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);
        var editUser = mapper.Map<EditProfileDto>(user);
        return View(editUser);
    }

    public async Task<IActionResult> SaveProfile(EditProfileDto model)
    {
        var user = await userManager.FindByNameAsync(User.Identity.Name);

        if (user == null)
        {
            return NotFound();
        }
        
        mapper.Map(model, user);

        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            fileService.DeleteFile(user.ProfilImageUrl);
            
            user.ProfilImageUrl = await fileService.UploadFileAsync(model.ImageFile,"images/users/profileImages");
        }

        if (!string.IsNullOrWhiteSpace(model.CurrentPassword) && !string.IsNullOrWhiteSpace(model.NewPassword))
        {
            if (string.IsNullOrWhiteSpace(model.CurrentPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
            {
                ModelState.AddModelError(string.Empty, "Şifre değiştirmek için hem mevcut şifrenizi hem de yeni şifrenizi girmelisiniz.");
                return View("Index", model);
            }

            if (model.NewPassword != model.ConfirmNewPassword)
            {
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
        
        return RedirectToAction("Index");
        
    }
    
}