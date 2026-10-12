using Blogy.Business.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents.Default_index;

public class DefaultBlogsComponent(ICategoryService categoryService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categoriesWithBlogs = await categoryService.GetCategoriesWithBlogsAsync();
        return View(categoriesWithBlogs);
    }
}