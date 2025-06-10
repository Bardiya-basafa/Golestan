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

    public ClassroomsController(IFacultyService facultyService, IClassroomService classroomService)
    {
        _facultyService = facultyService;
        _classroomService = classroomService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddClassroom(int facultyId)
    {
        var detailsFacultyDto = await _facultyService.GetDetailsFacultyById(facultyId);

        var model = new AddClassroomDto()
        {
            FacultyId = facultyId,
            FacultyName = detailsFacultyDto.MajorName,
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddClassroom(AddClassroomDto dto)
    {
        var result = await _classroomService.AddClassroom(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("ClassroomManagement", "Admin", dto.FacultyId);
        }

        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> ClassroomActions(int classroomId)
    {
        var model = await _classroomService.GetClassroomManagementDto(classroomId);

        return View(model);
    }

    public async Task<IActionResult> VerifyClassNumber(string classNumber, int facultyId)
    {
        var exist = await _classroomService.VerifyClassroomNumber(classNumber, facultyId);

        return Json(!exist);
    }

}
