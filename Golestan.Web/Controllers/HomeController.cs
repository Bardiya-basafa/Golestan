using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

public class HomeController : Controller {

    // GET
    public IActionResult Index()
    {
        return View();
    }

}
