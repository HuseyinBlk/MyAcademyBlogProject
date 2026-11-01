using Blogy.Entity.Entities;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blogy.WebUI.Areas.Admin.Controllers;

[Authorize(Roles = Roles.Admin)]
[Area(Roles.Admin)]
public class RoleController(RoleManager<AppRole> roleManager) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var roles = await roleManager.Roles.ToListAsync();
        return View(roles);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AppRole role)
    {
        var result = await roleManager.CreateAsync(role);
        
        if(!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return View(role);
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var role = await roleManager.FindByIdAsync(id.ToString());
        return View(role);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(AppRole role)
    {
        var existingRole = await roleManager.FindByIdAsync(role.Id.ToString());
        
        if (existingRole == null)
        {
            return NotFound();
        }
        
        existingRole.Name = role.Name;
        await roleManager.UpdateAsync(existingRole);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var role = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

        if (role == null)
        {
            return RedirectToAction("Index");
        }
        
        await roleManager.DeleteAsync(role);
        return RedirectToAction("Index");
        
    }
}