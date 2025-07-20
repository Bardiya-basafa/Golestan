namespace Golestan.Web.Components;

using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class StudentInfoViewComponent(IStudentService studentService) : ViewComponent {

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = GetUserId();
        var model = await studentService.GetStudentInfo(userId);

        return View(model);
    }
    public string? GetUserId()
    {
        return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

}
