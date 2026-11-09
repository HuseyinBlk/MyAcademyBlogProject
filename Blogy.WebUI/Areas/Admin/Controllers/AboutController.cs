using Blogy.Business.Dtos.AboutDtos;
using Blogy.Business.Services.AboutServices;
using Blogy.Business.Services.AiServices;
using Blogy.WebUI.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Roles.Admin)]
public class AboutController(IAboutService aboutService, IAiService aiService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var abouts = await aboutService.GetAllAsync();
        var about = abouts.FirstOrDefault();
        return View(about);
    }

    [HttpPost]
    public async Task<IActionResult> GenerateAiSummary(string? customPrompt)
    {
        var result = await aiService.GenerateAboutSummaryAsync(customPrompt);
        return Json(new { success = true, summary = result });
    }

    [HttpPost]
    public async Task<IActionResult> SaveSummary(int id, string summary)
    {
        if (string.IsNullOrWhiteSpace(summary))
        {
            return Json(new { success = false, message = "Tanıtım metni boş olamaz!" });
        }

        var abouts = await aboutService.GetAllAsync();
        var existing = (id > 0 ? abouts.FirstOrDefault(x => x.Id == id) : null) ?? abouts.FirstOrDefault();

        if (existing != null)
        {
            var updateDto = await aboutService.GetByIdAsync(existing.Id);
            updateDto.Description1 = summary;
            await aboutService.UpdateAsync(updateDto);
            return Json(new { success = true, message = "Tanıtım metni başarıyla güncellendi ve sitede yayınlandı!" });
        }
        else
        {

            await aboutService.CreateAsync(new CreateAboutDto
            {
                Title1 = "Hakkımızda",
                Description1 = summary,
                Title2 = "Vizyonumuz & Misyonumuz",
                Description2 = summary,
                FeatureTitle1 = "Kaliteli İçerik",
                FeatureTitle2 = "Güçlü Yazar Kadrosu",
                FeatureTitle3 = "Yenilikçi Bakış Açısı"
            });

            return Json(new { success = true, message = "Tanıtım metni başarıyla oluşturuldu ve sitede yayınlandı!" });
        }
    }
}
