namespace Golestan.Web.Components;

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class CurrentTermViewComponent : ViewComponent {

    private readonly ITermService _termService;

    public CurrentTermViewComponent(ITermService termService)
    {
        _termService = termService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await _termService.GetCurrentTerm();

        return View(model);
    }

}
