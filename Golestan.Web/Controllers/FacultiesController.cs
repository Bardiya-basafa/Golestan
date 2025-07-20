using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Faculty;
using Application.Interfaces;
using Base;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Shared.Constants;


public class FacultiesController : BaseController {

    private readonly IFacultyService _facultyService;

    public FacultiesController(IFacultyService facultyService) : base(facultyService)
    {
        _facultyService = facultyService;
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Index(int facultyId)
    {
        var faculty = await _facultyService.GetFacultyDtoById(facultyId);


        return View(faculty);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Update(int facultyId)
    {
        var faculty = await _facultyService.GetFacultyDtoById(facultyId);


        return View(faculty.Adapt<UpdateFacultyDto>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Update(UpdateFacultyDto model)
    {
        if (!ModelState.IsValid){
            return View(model);
        }

        var result = await _facultyService.UpdateFacutly(model.Adapt<FacultyDto>());
        ShowMessage(result.Message, result.Succeeded);

        if (!result.Succeeded){
            return View(model);
        }


        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Add()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Add(AddFacultyDto model)
    {
        var result = await _facultyService.AddFaculty(model.Adapt<FacultyDto>());

        ShowMessage(result.Message, result.Succeeded);

        if (!result.Succeeded){
            return View(model);
        }

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> VerifyMajor(string majorName)
    {
        var exist = await _facultyService.VerifyMajorName(majorName);

        return Json(!exist);
    }

    public async Task<IActionResult> VerifyBuildingName(string buildingName)
    {
        var exist = await _facultyService.VerifyBuildingName(buildingName);

        return Json(!exist);
    }

}
