using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Instructor;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;


public class InstructorsController : Controller {

    private readonly IFacultyService _facultyService;

    private readonly UserManager<AppUser> _userManager;

    public InstructorsController(IFacultyService facultyService, UserManager<AppUser> userManager)
    {
        _facultyService = facultyService;
        _userManager = userManager;
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

        return View();
    }

    public async Task<IActionResult> VerifyEmail(string email)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user != null){
            return Json(false);
        }

        return Json(true);
    }

}
