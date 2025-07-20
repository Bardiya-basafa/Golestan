namespace Golestan.Web.Components;

using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class InstructorInfoViewComponent : ViewComponent {

    private readonly IInstructorService _instructorService;

    public InstructorInfoViewComponent(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = GetUserId();
        var model = await _instructorService.GetInstructorInfo(userId);

        return View(model);
    }

    public string? GetUserId()
    {
        return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

}
