using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Student;
using Application.Interfaces;
using Application.Services;
using Base;


public class StudentsController : BaseController {

    private readonly IStudentService _studentService;

    private readonly IFacultyService _facultyService;

    private readonly IUserService _userService;


    public StudentsController(IStudentService studentService, IFacultyService facultyService, IUserService userService)
    {
        _studentService = studentService;
        _facultyService = facultyService;
        _userService = userService;
    }

    public IActionResult StudentDashboard()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddStudent(int facultyId)
    {
        var faculty = await _facultyService.GetDetailsFacultyById(facultyId);

        if (faculty == null){
            ShowMessage("Faculty not found", false);

            return RedirectToAction("ManageStudents", "Admin", routeValues: new { facultyId = facultyId });
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
    public async Task<IActionResult> AddStudent(AddStudentDto dto)
    {
        if (!ModelState.IsValid){
            return View(dto);
        }

        var result = await _userService.RegisterNewStudent(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("ManageStudents", "Admin", routeValues: new { facultyId = dto.FacultyId });
        }

        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> StudentSections(int studentId)
    {
        var model = await _studentService.GetStudentSections(studentId);

        return View(model);
    }

}
