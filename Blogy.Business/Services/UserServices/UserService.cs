using AutoMapper;
using Blogy.Business.Dtos.UserDtos;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Identity;

namespace Blogy.Business.Services.UserServices;

public class UserService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, IMapper mapper, RoleManager<AppRole> roleManager) : IUserService
{
    public async Task<IdentityResult> RegisterAsync(RegisterDto model)
    {
        var user = mapper.Map<AppUser>(model);

        var result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                await roleManager.CreateAsync(new AppRole { Name = Roles.User });
            }

            await userManager.AddToRoleAsync(user, Roles.User);
        }
        return result;
    }

    public async Task<SignInResult> LoginAsync(LoginDto model)
    {
        var result = await signInManager.PasswordSignInAsync(
            model.UserName,
            model.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        return result;
    }

    public async Task LogoutAsync()
    {
        await signInManager.SignOutAsync();
    }
}
