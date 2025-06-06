using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Section;
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

    [HttpGet]
    public async Task<IActionResult> AddSection()
    {
        var facultiesMajorNames = await _facultyService.GetFacultiesMajorNames();

        var model = new AddSectionDto()
        {
            FacultyMajorNames = facultiesMajorNames
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ClassroomOptions(int facultyId)
    {
        Dictionary<int, string>? classroomOptions = await _facultyService.GetFacultyClassrooms(facultyId);

        var options = classroomOptions.Select(kvp => new
        {
            value = kvp.Key,
            text = "Class number : " + kvp.Value
        }).ToList();

        return Json(options);
    }

    [HttpGet]
    public async Task<IActionResult> CourseOptions(int facultyId)
    {
        Dictionary<int, string>? courseOptions = await _facultyService.GetFacultyCourses(facultyId);

        var options = courseOptions.Select(kvp => new
        {
            value = kvp.Key,
            text = "Course : " + kvp.Value
        }).ToList();

        return Json(options);
    }

    [HttpGet]
    public async Task<IActionResult> InstructorOptions(int facultyId)
    {
        Dictionary<int, string>? instructorOptions = await _facultyService.GetFacultyInstructors(facultyId);

        var options = instructorOptions.Select(kvp => new
        {
            value = kvp.Key,
            text = $"#{kvp.Key} " + kvp.Value
        }).ToList();

        return Json(options);
    }


    public async Task<IActionResult> FacultiesManagement()
    {
        var faculties = await _facultyService.GetFaculties();


        return View(faculties);
    }

}
