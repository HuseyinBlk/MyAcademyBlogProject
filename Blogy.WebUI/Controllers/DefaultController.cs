using Blogy.Business.Services.CategoryServices;
using Blogy.DataAccess.Context;
using Microsoft.AspNetCore.Mvc;

namespace Blogy.WebUI.Controllers;

public class DefaultController : Controller
{

    public async Task<IActionResult> Index()
    {
        return View();
    }

}
