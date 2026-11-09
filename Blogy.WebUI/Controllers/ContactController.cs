using Blogy.Business.Dtos.ContactDtos;
using Blogy.Business.Services.ContactServices;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers;

public class ContactController(IContactService contactService) : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(CreateContactDto dto)
    {
        await  contactService.CreateAsync(dto);
        TempData["SuccessMessage"] = "Mesajınız başarıyla iletildi. En kısa sürede sizinle iletişime geçeceğiz.";
        return RedirectToAction("Index");
    }
}
