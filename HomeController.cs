using Microsoft.AspNetCore.Mvc;

namespace webcore_backend;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Content("WebCore działa!");
    }
}
