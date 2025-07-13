using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Instructor;
using Application.DTOs.Score;
using Application.Interfaces;
using Base;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;


public class InstructorsController : BaseController {

    private readonly IFacultyService _facultyService;

    private readonly UserManager<AppUser> _userManager;

    private readonly IUserService _userService;

    private readonly IInstructorService _instructorService;

    public InstructorsController(IFacultyService facultyService, UserManager<AppUser> userManager, IUserService userService, IInstructorService instructorService) : base(facultyService)
    {
        _facultyService = facultyService;
        _userManager = userManager;
        _userService = userService;
        _instructorService = instructorService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _instructorService.GetInstructorAppUser("ea2856a2-3089-47f2-9a8b-7b86de653ea4");

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Sections(int instructorId)
    {
        var model = await _instructorService.GetInstructorDtoById(instructorId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Students(int sectionId)
    {
        var model = await _instructorService.GetInstructorStudentsOfSection(sectionId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> SubmitStudentsScores(int sectionId)
    {
        var model = await _instructorService.GetExamResultsOfSection(sectionId);

        // invoke the view component async 

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitStudentsScores(ScoreDto model)
    {
        var result = await _instructorService.SubmitStudentScore(model);

        if (!result.Succeeded){
            ShowMessage(result.Message, result.Succeeded);
        }

        return RedirectToAction("SubmitStudentsScores", new { instructorId = model.InstructorId, sectionId = model.SectionId });
    }

    [HttpGet]
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
            return RedirectToAction("Instructors", "Admin");
        }


        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveCourseInstructor(int instructorId, int courseId)
    {
        var result = await _instructorService.RemoveCourseInstructor(instructorId, courseId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Index", "Courses", routeValues: new { courseId = courseId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int instructorId, int facultyId)
    {
        var result = await _instructorService.RemoveInstructor(instructorId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Instructors", "Admin", routeValues: new { facultyId = facultyId });
    }

}
