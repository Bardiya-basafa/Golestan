using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

public class FacultiesController : Controller {

    // GET
    public IActionResult Index()
    {
        return View();
    }

}
