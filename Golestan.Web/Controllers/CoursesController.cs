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

        var result = await _courseService.AddCourse(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("ManageCourses", "Admin", new { facultyId = dto.FacultyId });
        }


        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveCourse(int courseId, int facultyId)
    {
        var result = await _courseService.RemoveCourse(courseId);
        ShowMessage(result.Message,result.Succeeded);
        return RedirectToAction("ManageCourses", "Admin", new { facultyId = facultyId });
    }

    [HttpGet]
    public async Task<IActionResult> CourseActions(int courseId)
    {
        var model = await _courseService.GetCourseActionsDto(courseId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ApplyNewInstructorToCourse(int facultyId, int courseId)
    {
        var model = await _courseService.GetAllFacultyInstructors(facultyId, courseId);

        if (model.Instructors.Count == 0){
            ShowMessage("No instructor available for this course", false);

            return RedirectToAction("CourseActions", routeValues: new { courseId = courseId });
        }


        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApplyNewInstructorToCourse(ApplyNewInstructorDto dto)
    {
        if (!ModelState.ContainsKey("InstructorId") && !ModelState.ContainsKey("CourseId")){
            return View(dto);
        }

        var result = await _courseService.ApplyNewInstructorToCourse(dto);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("CourseActions", routeValues: new { courseId = dto.CourseId });
    }

}
