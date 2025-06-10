using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Student;
using Application.Interfaces;
using Application.Services;
using Base;


public class StudentsController : BaseController {

    private readonly IStudentService _studentService;

    private readonly IFacultyService _facultyService;


    public StudentsController(IStudentService studentService, IFacultyService facultyService)
    {
        _studentService = studentService;
        _facultyService = facultyService;
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

}
