using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

public class ErrorController : Controller {

    // GET
    public IActionResult Index(string errorMessage)
    {
        return View(errorMessage);
    }

}
