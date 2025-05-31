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
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddFaculty()
    {
        return View();
    }

}
