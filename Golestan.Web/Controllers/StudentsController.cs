using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Objection;
using Application.DTOs.Student;
using Application.Interfaces;
using Base;


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

    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();

        // var model = await _studentService.GetStudentUserApp(userId);

        var model = await _studentService.GetStudentUserApp("d8801a7e-52d4-4bdf-b519-6dfde3c04df0");

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Selection(int studentId)
    {
        var model = await _termService.GetCurrentTerm();
        ViewBag.studentId = studentId;

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> AvailableSections(int studentId)
    {
        var model = await _selectionService.GetAvailableSectionsForSelection(studentId);

        if (model == null){
            ShowMessage("The selection time passed, no sections available", false);

            return RedirectToAction("Selection");
        }

        return View(model);
    }

    [HttpGet]
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
    public async Task<IActionResult> Sections(int studentId)
    {
        var model = await _studentService.GetStudentSections(studentId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Exams(int studentId)
    {
        // var model = await _studentService.GetAllStudentTerms(studentId);
        ViewBag.studentId = studentId;

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> TermHistory(int studentId)
    {
        var model = await _studentService.GetAllStudentTerms(studentId);
        ViewBag.studentId = studentId;

        return View(model);
    }


    [HttpGet]
    public async Task<IActionResult> ActiveExamResults(int studentId)
    {
        var currentTerm = await _termService.GetCurrentTerm();

        if (currentTerm == null){
            ShowMessage("Currently there is no active term", false);

            return RedirectToAction("Exams", new { studentId = studentId });
        }


        return RedirectToAction("Results","Exam",new { studentId = studentId , termId = currentTerm.Id });
    }

    

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int studentId, int facultyId)
    {
        var result = await _studentService.Rmove(studentId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Students", "Admin", routeValues: new { facultyId = facultyId });
    }

}
