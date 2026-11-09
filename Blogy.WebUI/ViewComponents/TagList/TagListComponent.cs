using Blogy.Business.Services.TagServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents.TagList;

public class TagListComponent(ITagService tagService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var tags = await tagService.GetAllAsync();
        return View(tags);
    }
}
