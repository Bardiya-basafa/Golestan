using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

public class AdminController : Controller {

    // GET
    public IActionResult Index()
    {
        return View();
    }

}
