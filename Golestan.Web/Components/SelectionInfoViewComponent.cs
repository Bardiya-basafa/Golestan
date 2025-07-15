namespace Golestan.Web.Components;

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class SelectionInfoViewComponent : ViewComponent {

    private readonly ISelectionService _selectionService;

    public SelectionInfoViewComponent(ISelectionService selectionService)
    {
        _selectionService = selectionService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int studentId)
    {
        var model = await _selectionService.GetSelectionInfo(studentId);
        return View(model);
    }

}
