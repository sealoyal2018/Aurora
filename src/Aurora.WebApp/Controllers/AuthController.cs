using Microsoft.AspNetCore.Mvc;

namespace Aurora.WebApp.Controllers;

public class AuthController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
