using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Course;
using Application.Interfaces;
using Base;
using Domain.Entities;


public class CoursesController : BaseController {

    private readonly ICourseService _courseService;

    private readonly IFacultyService _facultyService;

    public CoursesController(ICourseService courseService, IFacultyService facultyService)
    {
        _courseService = courseService;
        _facultyService = facultyService;
    }

    [HttpGet]
    public async Task<IActionResult> AddCourse(int facultyId)
    {
        var faculty = await _facultyService.GetDetailsFacultyById(facultyId);


        var model = new AddCourseDto()
        {
            FacultyMajorName = faculty.MajorName,
            FacultyId = facultyId
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCourse(AddCourseDto dto)
    {
        if (!ModelState.IsValid){
            return View(dto);
        }

        // var result = await _courseService.AddCourse(dto);
        // ShowMessage(result.Message, result.Succeeded);

        if (true){
            return RedirectToAction("CourseManagement", "Admin", new { facultyId = dto.FacultyId });
        }


        return View(dto);
    }

}
