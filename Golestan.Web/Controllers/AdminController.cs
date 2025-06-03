using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.Interfaces;
using Application.Services;


public class AdminController : Controller {

    private readonly IFacultyService _facultyService;

    private readonly IInstructorService _instructorService;

    private readonly IClassroomService _classroomService;

    public AdminController(IFacultyService facultyService, IInstructorService instructorService, IClassroomService classroomService)
    {
        _facultyService = facultyService;
        _instructorService = instructorService;
        _classroomService = classroomService;
    }

    // GET
    public async Task<IActionResult> Dashboard()
    {
        var faculties = await _facultyService.GetFaculties();


        return View(faculties);
    }

    public async Task<IActionResult> InstructorsManagement()
    {
        var instructorsDto = await _instructorService.GetInstructors();

        return View(instructorsDto);
    }

    public async Task<IActionResult> ChooseFaculty()
    {
        var faculties = await _facultyService.GetFaculties();

        return View(faculties);
    }

    [HttpGet]
    public async Task<IActionResult> ManageFacultyClassrooms(int facultyId)
    {
        var dto = await _classroomService.GetFacultyClassrooms(facultyId);

        return View(dto);
    }

    public async Task<IActionResult> FacultiesManagement()
    {
        var faculties = await _facultyService.GetFaculties();


        return View(faculties);
    }

}
