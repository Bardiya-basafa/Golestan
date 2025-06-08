using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Base;


public class CoursesController : BaseController {

    public async Task<IActionResult> AddCourse()
    {
        return View();
    }

}
