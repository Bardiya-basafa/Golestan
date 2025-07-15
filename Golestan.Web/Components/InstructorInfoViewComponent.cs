namespace Golestan.Web.Components;

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class InstructorInfoViewComponent : ViewComponent {

    private readonly IInstructorService _instructorService;

    public InstructorInfoViewComponent(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int instructorId)
    {
        var model = await _instructorService.GetInstructorInfo(instructorId);

        return View(model);
    }

}
