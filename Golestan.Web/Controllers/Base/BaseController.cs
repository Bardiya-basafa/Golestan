using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers.Base;

using System.Security.Claims;
using Application.Interfaces;
using Shared.Constants;


public abstract class BaseController(IFacultyService facultyService) : Controller {

    public void ShowMessage(string? message, bool result)
    {
        TempData["NotificationMessage"] = message;
        TempData["Success"] = result;
    }

    public async Task SeedFacultyData(int facultyId)
    {
        var faculty = await facultyService.GetFacultyDtoById(facultyId);
        ViewBag.FacultyId = facultyId;
        ViewBag.FacultyName = faculty.MajorName;
    }

    public string? GetUserId()
    {
        return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

}
