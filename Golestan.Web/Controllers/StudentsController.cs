using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

public class StudentsController : Controller {

    // GET
    public IActionResult Index()
    {
        return View();
    }

}
