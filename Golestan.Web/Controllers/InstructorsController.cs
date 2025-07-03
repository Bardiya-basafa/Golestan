using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Instructor;
using Application.Interfaces;
using Base;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;


public class InstructorsController : BaseController {

    private readonly IFacultyService _facultyService;

    private readonly UserManager<AppUser> _userManager;

    private readonly IUserService _userService;

    private readonly IInstructorService _instructorService;

    public InstructorsController(IFacultyService facultyService, UserManager<AppUser> userManager, IUserService userService, IInstructorService instructorService)
    {
        _facultyService = facultyService;
        _userManager = userManager;
        _userService = userService;
        _instructorService = instructorService;
    }

    public IActionResult InstructorDashboard()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetInstructorSections(int instructorId)
    {
        var model = await _instructorService.GetInstructorSections(instructorId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetInstructorStudentsFoSection(int sectionId)
    {
        var model = await _instructorService.GetInstructorStudentsForSection(sectionId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> AdmitStudentsScores(int instructorId, int sectionId)
    {
        var model = await _instructorService.GetExamResultsForSection(instructorId, sectionId);

        // invoke the view component async 

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitStudentsScores(SubmitScoreDto dto)
    {
        var result = await _instructorService.SubmitStudentScore(dto);

        if (!result.Succeeded){
            ShowMessage(result.Message, result.Succeeded);
        }

        return RedirectToAction("AdmitStudentsScores", new { instructorId = dto.InstructorId, sectionId = dto.SectionId });
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
        if (!ModelState.IsValid){
            var facultyOptions = await _facultyService.GetFacultiesMajorNames();
            dto.FacultyOptions = facultyOptions;

            return View(dto);
        }

        var result = await _userService.RegisterNewInstructor(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("ManageInstructors", "Admin");
        }


        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveCourseInstructor(int instructorId, int courseId)
    {
        var result = await _instructorService.RemoveCourseInstructor(instructorId, courseId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("CourseActions", "Courses", routeValues: new { courseId = courseId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveInstructor(int instructorId, int facultyId)
    {
        var result = await _instructorService.RemoveInstructor(instructorId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("ManageInstructors", "Admin", routeValues: new { facultyId = facultyId });
    }

}
