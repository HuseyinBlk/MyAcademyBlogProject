using AutoMapper;
using Blogy.Business.Dtos.CategoryDtos;
using Blogy.Business.Services.CategoryServices;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers;

[Authorize(Roles = Roles.Admin)]
[Area(Roles.Admin)]
public class CategoryController(ICategoryService _categoryService, IMapper _mapper) : Controller
{
    
    // GET
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();
        return View(categories);
    }
    
    public async Task<IActionResult> Create(){
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return View(categoryDto);
        }
        
        await _categoryService.CreateAsync(categoryDto);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        
        if (category == null)
        {
            return NotFound();
        }
        
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateCategoryDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return View(categoryDto);
        }
        
        await _categoryService.UpdateAsync(categoryDto);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        await  _categoryService.DeleteAsync(id);
        return RedirectToAction("Index"); 
    }
    
    
}