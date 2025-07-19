namespace Golestan.Web.Components;

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class CurrentTermViewComponent : ViewComponent {

    private readonly ITermService _termService;

    public CurrentTermViewComponent(ITermService termService)
    {
        _termService = termService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int termId)
    {
        if (termId == 0){
            return View(await _termService.GetCurrentTerm());
        }

        var model = await _termService.GetTermById(termId);

        return View(model);
    }

}
