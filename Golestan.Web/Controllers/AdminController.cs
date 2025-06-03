using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.Interfaces;


public class AdminController : Controller {

    private readonly IFacultyService _facultyService;

    public AdminController(IFacultyService facultyService)
    {
        _facultyService = facultyService;
    }

    // GET
    public async Task<IActionResult> Dashboard()
    {
        var faculties = await _facultyService.GetFaculties();


        return View(faculties);
    }

    public IActionResult InstructorsManagement()
    {
        return View();
    }

    public IActionResult FacultiesManagement()
    {
        return View();
    }

}
