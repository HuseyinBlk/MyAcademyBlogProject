using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers;

public class UiLayoutController : Controller
{

    public IActionResult Layout()
    {
        return View();
    }
}
