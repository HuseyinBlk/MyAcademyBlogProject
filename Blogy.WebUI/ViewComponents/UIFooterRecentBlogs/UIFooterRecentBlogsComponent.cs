using Blogy.Business.Services.BlogServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents.UIFooterRecentBlogs;

public class UIFooterRecentBlogsComponent(IBlogService blogService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var blogs = await blogService.GetLast3BlogsAsync();
        return View(blogs);
    }
}
