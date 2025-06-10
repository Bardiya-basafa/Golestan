using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs;
using Application.DTOs.Faculty;
using Application.Interfaces;
using Domain.Enums;
using Newtonsoft.Json;


public class FacultiesController : Controller {

    private readonly IFacultyService _facultyService;

    public FacultiesController(IFacultyService facultyService)
    {
        _facultyService = facultyService;
    }


    public async Task<IActionResult> FacultyManagement(int id)
    {
        var faculty = await _facultyService.GetDetailsFacultyById(id);

        if (faculty == null){
            RedirectToAction("Index", "Faculties");
        }

        return View(faculty);
    }

    [HttpGet]
    public async Task<IActionResult> EditFaculty(int id)
    {
        var faculty = await _facultyService.GetEditFacultyById(id);

        if (faculty == null){
            RedirectToAction("Index", "Faculties");
        }

        return View(faculty);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFaculty(EditFacultyDto dto)
    {
        if (!ModelState.IsValid){
            return View(dto);
        }

        var result = await _facultyService.EditFaculty(dto);

        if (!result){
            return RedirectToAction("Index", "Error", new { message = "Faculty could not be updated" });
        }

        TempData["NotificationMessage"] = "Faculty edited successfully.";
        TempData["Success"] = true;

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddFaculty(AddFacultyDto faculty)
    {
        var result = await _facultyService.AddFaculty(faculty);

        if (result){
            TempData["NotificationMessage"] = "Faculty added successfully.";
            TempData["Success"] = true;

            return RedirectToAction("Index", "Faculties");
        }

        return RedirectToAction("Index", "Error", new { message = "Faculty could not be added." });
    }

    [HttpGet]
    public async Task<IActionResult> AddFaculty()
    {
        return View();
    }

    public async Task<IActionResult> VerifyMajor(string majorName)
    {
        var exist = await _facultyService.VerifyMajor(majorName);

        return Json(!exist);
    }

    public async Task<IActionResult> VerifyBuildingName(string buildingName)
    {
        var exist = await _facultyService.VerifyBuilding(buildingName);

        return Json(!exist);
    }

}
