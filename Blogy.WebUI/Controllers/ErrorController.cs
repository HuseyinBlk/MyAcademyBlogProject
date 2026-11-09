using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers;

public class ErrorController : Controller
{

    public IActionResult AccessDenied()
    {
        return View();
    }
}
