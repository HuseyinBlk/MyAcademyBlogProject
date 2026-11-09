using Blogy.Business.Services.AboutServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.ViewComponents.UIFooterAboutComponent;

public class UIFooterAboutComponent(IAboutService aboutService) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var abouts = await aboutService.GetAllAsync();
        var about = abouts.FirstOrDefault();

        var summary = about?.Description1 ?? "Blogy, modern yazılım teknolojileri ve zengin içerikleri okuyucularıyla buluşturan profesyonel bir blog platformudur.";

        return View(model: summary);
    }
}
