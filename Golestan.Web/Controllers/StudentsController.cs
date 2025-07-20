using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Objection;
using Application.DTOs.Student;
using Application.Interfaces;
using Base;
using Microsoft.AspNetCore.Authorization;
using Shared.Constants;


public class StudentsController : BaseController {

    private readonly IStudentService _studentService;

    private readonly IFacultyService _facultyService;

    private readonly IUserService _userService;

    private readonly ITermService _termService;

    private readonly ISelectionService _selectionService;

    private readonly IExamService _examService;


    public StudentsController(IStudentService studentService, IFacultyService facultyService, IUserService userService, ITermService termService, ISelectionService selectionService, IExamService examService) : base(facultyService)
    {
        _studentService = studentService;
        _facultyService = facultyService;
        _userService = userService;
        _termService = termService;
        _selectionService = selectionService;
        _examService = examService;
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> Selection()
    {
        var model = await _termService.GetCurrentTerm();

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> AvailableSections()
    {
        var userId = GetUserId();
        var model = await _selectionService.GetAvailableSectionsForSelection(userId);

        if (model == null){
            ShowMessage("The selection time passed, no sections available", false);

            return RedirectToAction("Selection");
        }

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Add(int facultyId)
    {
        var faculty = await _facultyService.GetFacultyDtoById(facultyId);

        if (faculty == null){
            ShowMessage("Faculty not found", false);

            return RedirectToAction("Students", "Admin", routeValues: new { facultyId = facultyId });
        }

        var model = new AddStudentDto()
        {
            FacultyId = faculty.Id,
            FacultyName = faculty.MajorName
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Add(AddStudentDto dto)
    {
        if (!ModelState.IsValid){
            return View(dto);
        }

        var result = await _userService.RegisterNewStudent(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("Students", "Admin", routeValues: new { facultyId = dto.FacultyId });
        }

        return View(dto);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> Sections()
    {
        var userId = GetUserId();
        var model = await _studentService.GetStudentSections(userId);

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public IActionResult Exams()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> TermHistory()
    {
        var userId = GetUserId();
        var model = await _studentService.GetAllStudentTerms(userId);

        return View(model);
    }


    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> ActiveExamResults()
    {
        var currentTerm = await _termService.GetCurrentTerm();

        if (currentTerm == null){
            ShowMessage("Currently there is no active term", false);

            return RedirectToAction("Exams");
        }


        return RedirectToAction("Results", "Exam", new { termId = currentTerm.Id });
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Remove(int studentId, int facultyId)
    {
        var result = await _studentService.Rmove(studentId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Students", "Admin", routeValues: new { facultyId = facultyId });
    }

}
