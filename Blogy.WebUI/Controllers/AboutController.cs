using Blogy.Business.Services.AboutServices;
using Blogy.Business.Services.AiServices;
using Blogy.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers;

public class AboutController(IAboutService aboutService, IAiService aiService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var abouts = await aboutService.GetAllAsync();
        var about = abouts.FirstOrDefault();
        var topWriters = await aboutService.GetTop6WritersAsync();

        var aiSummary = await aiService.GenerateAboutSummaryAsync();

        var viewModel = new AboutViewModel
        {
            About = about,
            TopWriters = topWriters,
            AiSummary = aiSummary
        };

        return View(viewModel);
    }
}
