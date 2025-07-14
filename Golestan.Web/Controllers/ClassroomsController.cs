using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Classroom;
using Application.Interfaces;
using Application.Services;
using Base;
using Domain.Entities;


public class ClassroomsController : BaseController {

    private readonly IFacultyService _facultyService;

    private readonly IClassroomService _classroomService;

    public ClassroomsController(IFacultyService facultyService, IClassroomService classroomService) : base(facultyService)
    {
        _facultyService = facultyService;
        _classroomService = classroomService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int classroomId)
    {
        var model = await _classroomService.GetClassroomById(classroomId);

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Add(int facultyId)
    {
        var detailsFacultyDto = await _facultyService.GetFacultyDtoById(facultyId);

        var model = new AddClassroomDto()
        {
            FacultyId = facultyId,
            FacultyName = detailsFacultyDto.MajorName,
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddClassroomDto dto)
    {
        var result = await _classroomService.AddClassroom(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("Classrooms", "Admin", dto.FacultyId);
        }

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int classroomId)
    {
        var result = await _classroomService.RemoveClassroom(classroomId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Classrooms", "Admin", routeValues: new { facultyId = 1 });
    }


    public async Task<IActionResult> VerifyClassNumber(string classNumber, int facultyId)
    {
        var exist = await _classroomService.VerifyClassroomNumber(classNumber, facultyId);

        return Json(!exist);
    }

}
