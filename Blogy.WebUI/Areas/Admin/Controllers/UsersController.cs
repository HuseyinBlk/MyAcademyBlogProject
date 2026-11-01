using AutoMapper;
using Blogy.Business.Dtos.UserDtos;
using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogy.WebUI.Areas.Admin.Controllers;

[Authorize(Roles = Roles.Admin)]
[Area(Roles.Admin)]
public class UsersController(UserManager<AppUser> userManager,RoleManager<AppRole> roleManager, IMapper mapper) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var users = await userManager.Users.ToListAsync();
        var mappedUsers = mapper.Map<List<ResultUserDto>>(users);

        foreach (var userDto in mappedUsers)
        {
            var appUser = users.First(x => x.Id == userDto.Id);
            userDto.Roles = await userManager.GetRolesAsync(appUser);
        }
        
        return View(mappedUsers);
    }

    public async Task<IActionResult> AssignRole(int id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        var roles = await roleManager.Roles.ToListAsync();
        
        var userRoles = await userManager.GetRolesAsync(user);
        
        var roleAssignList = new List<AssignRoleDto>();

        foreach (var role in roles)
        {
            roleAssignList.Add(new AssignRoleDto
            {
                Id = role.Id,
                RoleName = role.Name!,
                RoleExist = userRoles.Contains(role.Name!)
            });
        }
        
        return View(roleAssignList);
    }

    [HttpPost]
    public async Task<IActionResult> AssignRole(int id, List<string> selectedRoles)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        
        if (user == null) return NotFound();

        selectedRoles ??= new List<string>();
        
        var currentRoles = await userManager.GetRolesAsync(user);
        
        await userManager.AddToRolesAsync(user, selectedRoles.Except(currentRoles));
        
        await userManager.RemoveFromRolesAsync(user, currentRoles.Except(selectedRoles));

        
        return RedirectToAction("Index");
    }
}