using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Instructor;
using Application.Interfaces;


public class InstructorsController : Controller {

    private readonly IFacultyService _facultyService;

    public InstructorsController(IFacultyService facultyService)
    {
        _facultyService = facultyService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddInstructor(int id)
    {
        var facultyOptions = await _facultyService.GetFacultiesMajorNames();

        AddInstructorDto dto = new AddInstructorDto()
        {
            FacultyOptions = facultyOptions,
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddInstructor(AddInstructorDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        return View();
    }

}
