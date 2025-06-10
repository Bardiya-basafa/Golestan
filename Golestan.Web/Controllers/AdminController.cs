using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Section;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;


public class AdminController : Controller {

    private readonly IFacultyService _facultyService;

    private readonly IInstructorService _instructorService;

    private readonly IClassroomService _classroomService;

    private readonly ICourseService _courseService;

    private readonly ISectionService _sectionService;

    private readonly IStudentService _studentService;

    private readonly UserManager<AppUser> _userManager;


    public AdminController(IFacultyService facultyService, IInstructorService instructorService, IClassroomService classroomService, ICourseService courseService, ISectionService sectionService, IStudentService studentService, UserManager<AppUser> userManager)
    {
        _facultyService = facultyService;
        _instructorService = instructorService;
        _classroomService = classroomService;
        _courseService = courseService;
        _sectionService = sectionService;
        _studentService = studentService;
        _userManager = userManager;
    }

    // Admin Dashboard
    public async Task<IActionResult> AdminDashboard()
    {
        var faculties = await _facultyService.GetFaculties();


        return View(faculties);
    }

    // Managing each section
    public async Task<IActionResult> ManageStudents(int facultyId)
    {
        var model = await _studentService.GetFacultyStudents(facultyId);

        return View(model);
    }

    public async Task<IActionResult> ManageInstructors(int facultyId)
    {
        var instructorsDto = await _instructorService.GetFacultyInstructors();

        return View(instructorsDto);
    }

    [HttpGet]
    public async Task<IActionResult> ManageCourses(int facultyId)
    {
        var model = await _courseService.GetFacultyCourses(facultyId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ManageClassrooms(int facultyId)
    {
        var model = await _classroomService.GetFacultyClassrooms(facultyId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ManageSections(int facultyId)
    {
        var model = await _sectionService.GetFacultySections(facultyId);

        return View(model);
    }


    // Total overview at the system resources
    public async Task<IActionResult> AllClassrooms()
    {
        var model = await _facultyService.GetFaculties();

        return View(model);
    }

    public async Task<IActionResult> AllInstructors()
    {
        var model = await _facultyService.GetFaculties();

        return View(model);
    }

    public async Task<IActionResult> AllCourses()
    {
        var model = await _facultyService.GetFaculties();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> AllSections()
    {
        var model = await _facultyService.GetFaculties();

        return View(model);
    }
    [HttpGet]
    public async Task<IActionResult> AllStudents()
    {
        var model = await _facultyService.GetFaculties();

        return View(model);
    }
    public async Task<IActionResult> AllFaculties()
    {
        var faculties = await _facultyService.GetFaculties();


        return View(faculties);
    }

    // Ajaxs 

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
    public async Task<IActionResult> InstructorOptions(int courseId)
    {
        var instructorOptions = await _courseService.GetCourseInstructors(courseId);

        var options = instructorOptions.Select(kvp => new
        {
            value = kvp.Key,
            text = $"#{kvp.Key} " + kvp.Value
        }).ToList();

        return Json(options);
    }


   

    public async Task<IActionResult> VerifyEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null){
            return Json(false);
        }

        return Json(true);
    }

}
