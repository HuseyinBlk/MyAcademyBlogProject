using Blogy.Business.Services.BlogServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents.BlogsByCategory;

public class BlogsByCategoryLatestBlogsComponent(IBlogService blogService): ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var last3blog = await blogService.GetLast3BlogsAsync();
        return View(last3blog);
    }
}