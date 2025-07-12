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


    public StudentsController(IStudentService studentService, IFacultyService facultyService, IUserService userService) : base(facultyService)
    {
        _studentService = studentService;
        _facultyService = facultyService;
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
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
    public async Task<IActionResult> Terms(int studentId)
    {
        var model = await _studentService.GetAllStudentTerms(studentId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ExamResults(int termId, int studentId)
    {
        var model = await _studentService.GetAllTermExamResults(termId, studentId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ActiveExamResults(int studentId)
    {
        var model = await _studentService.GetActiveExamResults(studentId);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Objection(ObjectionDto dto)
    {
        var model = await _studentService.SubmitObjection(dto);

        return View(model);
    }

}
