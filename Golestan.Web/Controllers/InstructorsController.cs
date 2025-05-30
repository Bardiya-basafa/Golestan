using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

public class InstructorsController : Controller {

    // GET
    public IActionResult Index()
    {
        return View();
    }

}
