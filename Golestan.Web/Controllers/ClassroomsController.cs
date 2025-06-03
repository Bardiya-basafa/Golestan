using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

public class ClassroomsController : Controller {

    // GET
    public IActionResult Index()
    {
        return View();
    }
    

}
