namespace Golestan.Web.Controllers;

using Base;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;
using ISelectionService=Application.Interfaces.ISelectionService;


public class SelectionController : BaseController {

    private readonly ISelectionService _selectionService;

    public SelectionController(ISelectionService selectionService)
    {
        _selectionService = selectionService;
    }

    public async Task<IActionResult> SelectionTermDetails()
    {
        var model = await _selectionService.SelectionTermDetails();

        if (model.Result.Succeeded){
            return View(model);
        }

        ShowMessage(model.Result.Message, model.Result.Succeeded);

        return RedirectToAction("StudentDashboard", "Students");
    }

    public async Task<IActionResult> GetAvailableSectionForSelection(int studentId)
    {
        var model = await _selectionService.GetAvailableSectionsForSelection(studentId);

        if (model == null){
            ShowMessage("Right now selection is not available", false);

            return RedirectToAction("StudentDashboard", "Students");
        }

        return View(model);
    }

}
