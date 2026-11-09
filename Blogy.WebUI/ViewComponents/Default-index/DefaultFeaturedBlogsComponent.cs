using Blogy.Business.Services.BlogServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents.Default_index;

public class DefaultFeaturedBlogsComponent(IBlogService blogService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var blogs = await blogService.GetLast5BlogsAsync();
        return View(blogs);
    }
}
