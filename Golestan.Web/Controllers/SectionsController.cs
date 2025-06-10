using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Section;
using Application.Interfaces;
using Base;


public class SectionsController : BaseController {

    private readonly ISectionService _sectionService;

    private readonly ICourseService _courseService;

    private readonly IFacultyService _facultyService;

    public SectionsController(ISectionService sectionService, ICourseService courseService, IFacultyService facultyService)
    {
        _sectionService = sectionService;
        _courseService = courseService;
        _facultyService = facultyService;
    }

    [HttpGet]

    // GET
    public async Task<IActionResult> AddSection(int facultyId)
    {
        var facultyDetails = await _facultyService.GetDetailsFacultyById(facultyId);

        if (facultyDetails.CoursesCount == 0 || facultyDetails.InstructorsCount == 0 || facultyDetails.ClassesCount == 0){
            ShowMessage("There are no course or instructor in this faculty.", false);

            return RedirectToAction("SectionManagement", "Admin", routeValues: new { facultyId = facultyId });
        }

        var classrooms = await _facultyService.GetFacultyClassrooms(facultyId);
        var courses = await _facultyService.GetFacultyCourses(facultyId);

        var model = new AddSectionDto()
        {
            FacultyId = facultyId,
            Classrooms = classrooms,
            Courses = courses
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSection(AddSectionDto dto)
    {
        var result = await _sectionService.AddNewSection(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("SectionManagement", "Admin", routeValues: new { facultyId = dto.FacultyId });
        }

        return View(dto);
    }

    public async Task<IActionResult> InstructorOptions(int courseId)
    {
        var instructorOptions = await _courseService.GetCourseInstructors(courseId);

        if (instructorOptions == null || instructorOptions.Count() == 0){
            ShowMessage("There is no instructor options for this course", false);
        }

        var options = instructorOptions.Select(kvp => new
        {
            value = kvp.Key,
            text = $"#{kvp.Key} " + kvp.Value
        }).ToList();


        return Json(options);
    }

}
