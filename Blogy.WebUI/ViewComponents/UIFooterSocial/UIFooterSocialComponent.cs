using Blogy.Business.Services.SocialServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents.UIFooterSocialComponent;

public class UIFooterSocialComponent(ISocialService socialService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var socials = await socialService.GetAllAsync();
        return View(socials);
    }
}
