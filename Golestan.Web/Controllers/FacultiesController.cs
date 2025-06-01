using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;


public class FacultiesController : Controller {

    private readonly IFacultyService _facultyService;

    public FacultiesController(IFacultyService facultyService)
    {
        _facultyService = facultyService;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var faculties = await _facultyService.GetFaculties();

        return View(faculties);
    }

    public async Task<IActionResult> Details(int? id)
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddFaculty(AddFacultyDto faculty)
    {
        var result = await _facultyService.AddFaculty(faculty);

        if (result){
            return RedirectToAction("Index", "Faculties");
        }

        return RedirectToAction("Index", "Error", new { message = "Faculty could not be added." });
    }

    [HttpGet]
    public async Task<IActionResult> AddFaculty()
    {
        return View();
    }

    public async Task<IActionResult> VerifyMajor(string major)
    {
        var exist = await _facultyService.VerifyMajor(major);

        return Json(!exist);
    }

    public async Task<IActionResult> VerifyBuildingName(string buildingName)
    {
        var exist = await _facultyService.VerifyBuilding(buildingName);

        return Json(!exist);
    }

}
