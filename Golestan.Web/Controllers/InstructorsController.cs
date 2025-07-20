using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Instructor;
using Application.DTOs.Score;
using Application.Interfaces;
using Base;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Shared.Constants;


public class InstructorsController : BaseController {

    private readonly IFacultyService _facultyService;

    private readonly UserManager<AppUser> _userManager;

    private readonly IUserService _userService;

    private readonly IInstructorService _instructorService;

    private readonly ITermService _termService;

    public InstructorsController(IFacultyService facultyService, UserManager<AppUser> userManager, IUserService userService, IInstructorService instructorService, ITermService termService) : base(facultyService)
    {
        _facultyService = facultyService;
        _userManager = userManager;
        _userService = userService;
        _instructorService = instructorService;
        _termService = termService;
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Instructor)]
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();


        var model = await _instructorService.GetInstructorAppUser(userId);

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Instructor)]
    public async Task<IActionResult> Sections()
    {
        var userId = GetUserId();
        var model = await _instructorService.GetInstructorDtoById(userId);

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Instructor)]
    public async Task<IActionResult> Students(int sectionId)
    {
        var userId = GetUserId();
        var model = await _instructorService.GetInstructorStudentsOfSection(sectionId , userId);

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Instructor)]
    public async Task<IActionResult> ExamSections()
    {
        var userId = GetUserId();
        var model = await _instructorService.GetInstructorDtoById(userId);

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Instructor)]
    public async Task<IActionResult> Exams()
    {
        var model = await _termService.GetCurrentTerm();

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Instructor)]
    public async Task<IActionResult> TermHistory()
    {
        var userId = GetUserId();
        var model = await _instructorService.GetAllInstrcutorTerms(userId);

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Instructor)]
    public async Task<IActionResult> Courses()
    {
        var userId = GetUserId();
        var model = await _instructorService.GetCourses(userId);

        return View(model);
    }


    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Add()
    {
        var facultyOptions = await _facultyService.GetFacultiesMajorNamesOptions();

        AddInstructorDto dto = new AddInstructorDto()
        {
            FacultyOptions = facultyOptions,
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Add(AddInstructorDto dto)
    {
        if (!ModelState.IsValid){
            var facultyOptions = await _facultyService.GetFacultiesMajorNamesOptions();
            dto.FacultyOptions = facultyOptions;

            return View(dto);
        }

        var result = await _userService.RegisterNewInstructor(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("Instructors", "Admin", new { facultyId = dto.FacultyId });
        }


        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RemoveCourseInstructor(int instructorId, int courseId)
    {
        var result = await _instructorService.RemoveCourseInstructor(instructorId, courseId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Index", "Courses", routeValues: new { courseId = courseId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Remove(int instructorId, int facultyId)
    {
        var result = await _instructorService.RemoveInstructor(instructorId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Instructors", "Admin", routeValues: new { facultyId = facultyId });
    }

}
