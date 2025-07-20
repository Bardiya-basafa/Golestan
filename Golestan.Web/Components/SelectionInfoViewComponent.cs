namespace Golestan.Web.Components;

using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class SelectionInfoViewComponent : ViewComponent {

    private readonly ISelectionService _selectionService;

    public SelectionInfoViewComponent(ISelectionService selectionService)
    {
        _selectionService = selectionService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = GetUserId();
        var model = await _selectionService.GetSelectionInfo(userId);
        return View(model);
    }
    public string? GetUserId()
    {
        return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

}
