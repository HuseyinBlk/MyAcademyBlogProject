using Blogy.Business.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents.Category;

public class CategoryListComponent(ICategoryService categoryService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await categoryService.GetCategoriesWithBlogsAsync();
        return View(categories);
    }
}
