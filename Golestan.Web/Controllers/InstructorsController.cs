using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Instructor;
using Application.Interfaces;
using Base;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;


public class InstructorsController : BaseController {

    private readonly IFacultyService _facultyService;

    private readonly UserManager<AppUser> _userManager;

    private readonly IUserService _userService;

    public InstructorsController(IFacultyService facultyService, UserManager<AppUser> userManager, IUserService userService)
    {
        _facultyService = facultyService;
        _userManager = userManager;
        _userService = userService;
    }

    // GET
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> AddInstructor(int id)
    {
        var facultyOptions = await _facultyService.GetFacultiesMajorNames();

        AddInstructorDto dto = new AddInstructorDto()
        {
            FacultyOptions = facultyOptions,
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddInstructor(AddInstructorDto dto)
    {
        if (!ModelState.IsValid){
            var facultyOptions = await _facultyService.GetFacultiesMajorNames();
            dto.FacultyOptions = facultyOptions;

            return View(dto);
        }

        var result = await _userService.RegisterNewInstructor(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("ManageInstructors", "Admin");
        }


        return View(dto);
    }


}
