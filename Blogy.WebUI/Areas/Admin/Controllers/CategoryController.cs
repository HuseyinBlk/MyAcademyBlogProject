using AutoMapper;
using Blogy.Business.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController(ICategoryService _categoryService, IMapper _mapper) : Controller
{
    
    // GET
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllAsync();
        return View(categories);
    }
}