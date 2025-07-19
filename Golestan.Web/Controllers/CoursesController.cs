using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Course;
using Application.Interfaces;
using Base;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Shared.Constants;


public class CoursesController : BaseController {

    private readonly ICourseService _courseService;

    private readonly IFacultyService _facultyService;

    private readonly ITermService _termService;

    public CoursesController(ICourseService courseService, IFacultyService facultyService, ITermService termService) : base(facultyService)
    {
        _courseService = courseService;
        _facultyService = facultyService;
        _termService = termService;
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Index(int courseId)
    {
        var model = await _courseService.GetCourseDtoById(courseId);

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Add(int facultyId)
    {
        var faculty = await _facultyService.GetFacultyDtoById(facultyId);


        var model = new AddCourseDto()
        {
            FacultyMajorName = faculty.MajorName,
            FacultyId = facultyId,
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Add(AddCourseDto dto)
    {
        if (!ModelState.IsValid){
            return View(dto);
        }

        var result = await _courseService.AddCourse(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("Courses", "Admin", new { facultyId = dto.FacultyId });
        }


        return View(dto);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SetExamTime(int courseId)
    {
        var currentTerm = await _termService.GetCurrentTerm();

        if (currentTerm == null){
            ShowMessage("Currently you have not any open term", false);

            return RedirectToAction("Index", "Courses", new { courseId = courseId });
        }


        var model = new SetExamForCourseDto()
        {
            CourseId = courseId,
            CourseName = _courseService.GetCourseDtoById(courseId).GetAwaiter().GetResult().CourseName,
            ExamStartDate = currentTerm.ExamsStartTime,
            ExamEndDate = currentTerm.ExamsEndTime,
        };

        var classrooms = await _courseService.GetExamClassrooms(courseId);

        if (classrooms == null){
            ShowMessage("No class available for exam", false);

            return RedirectToAction("Index", "Courses", new { courseId = courseId });
        }

        model.Classrooms = classrooms;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SetExamTime(SetExamForCourseDto model)
    {
        var result = await _courseService.SetExam(model);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("Index", routeValues: new { courseId = model.CourseId });
        }


        model.CourseName = _courseService.GetCourseDtoById(model.CourseId).GetAwaiter().GetResult().CourseName;
        model.Classrooms = await _courseService.GetExamClassrooms(model.CourseId);
        var currentTerm = await _termService.GetCurrentTerm();
        model.ExamStartDate = currentTerm.ExamsStartTime;
        model.ExamEndDate = currentTerm.ExamsEndTime;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Remove(int courseId, int facultyId)
    {
        var result = await _courseService.RemoveCourse(courseId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Courses", "Admin", new { facultyId = facultyId });
    }


    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SetInstructor(int facultyId, int courseId)
    {
        var model = await _courseService.GetAvailableInsturctorsForCourse(facultyId, courseId);

        if (model.Instructors.Count == 0){
            ShowMessage("No instructor available for this course", false);

            return RedirectToAction("Index", routeValues: new { courseId = courseId });
        }


        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SetInstructor(CourseInstructorDto dto)
    {
        if (!ModelState.ContainsKey("InstructorId") && !ModelState.ContainsKey("CourseId")){
            return View(dto);
        }

        var result = await _courseService.ApplyInstructorToCourse(dto);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Index", routeValues: new { courseId = dto.CourseId });
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SetPrerequisite(int courseId)
    {
        var model = await _courseService.GetAvailableCoursesForPrerequisite(courseId);
        var course = await _courseService.GetCourseDtoById(courseId);
        ViewBag.courseid = course.Id;
        ViewBag.coursename = course.CourseName;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> SetPrerequisite(int courseId, int prerequisiteCourseId)
    {
        var result = await _courseService.AddPrerequisiteToCourse(courseId, prerequisiteCourseId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Index", routeValues: new { courseId = courseId });
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> RemovePrerequisite(int courseId, int prerequisiteCourseId)
    {
        var result = await _courseService.RemovePrerequisiteFromCourse(courseId, prerequisiteCourseId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Index", routeValues: new { courseId = courseId });
    }

}
