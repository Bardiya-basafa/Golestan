using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Faculty;
using Application.Interfaces;
using Base;
using Mapster;


public class FacultiesController : BaseController {

    private readonly IFacultyService _facultyService;

    public FacultiesController(IFacultyService facultyService) : base(facultyService)
    {
        _facultyService = facultyService;
    }


    public async Task<IActionResult> FacultyManagement(int facultyId)
    {
        var faculty = await _facultyService.GetFacultyDtoById(facultyId);


        return View(faculty);
    }

    [HttpGet]
    public async Task<IActionResult> EditFaculty(int facultyId)
    {
        var faculty = await _facultyService.GetFacultyDtoById(facultyId);


        return View(faculty.Adapt<UpdateFacultyDto>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFaculty(UpdateFacultyDto model)
    {
        if (!ModelState.IsValid){
            return View(model);
        }

        var result = await _facultyService.UpdateFacutly(model.Adapt<FacultyDto>());
        ShowMessage(result.Message, result.Succeeded);

        if (!result.Succeeded){
            return View(model);
        }


        return RedirectToAction("FacultyManagement");
    }

    [HttpGet]
    public async Task<IActionResult> AddFaculty()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddFaculty(AddFacultyDto model)
    {
        var result = await _facultyService.AddFaculty(model.Adapt<FacultyDto>());

        ShowMessage(result.Message, result.Succeeded);

        if (!result.Succeeded){
            return View(model);
        }

        return RedirectToAction("FacultyManagement");
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
