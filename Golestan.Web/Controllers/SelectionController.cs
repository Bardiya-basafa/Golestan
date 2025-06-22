namespace Golestan.Web.Controllers;

using Base;
using Microsoft.AspNetCore.Mvc;
using ISelectionService=Application.Interfaces.ISelectionService;


public class SelectionController(ISelectionService selectionService) : BaseController {

    public async Task<IActionResult> SelectionTermDetails()
    {
        var model = await selectionService.SelectionTermDetails();

        if (model.Result.Succeeded){
            return View(model);
        }

        ShowMessage(model.Result.Message, model.Result.Succeeded);

        return RedirectToAction("StudentDashboard", "Students");
    }

    public async Task<IActionResult> GetAvailableSectionForSelection(int studentId)
    {
        var model = await selectionService.GetAvailableSectionsForSelection(studentId);

        if (model == null){
            ShowMessage("Right now selection is not available", false);

            return RedirectToAction("StudentDashboard", "Students");
        }

        return View(model);
    }

    public async Task<IActionResult> SelectSection(int studentId, int sectionId)
    {
        var result = await selectionService.SelectSection(studentId, sectionId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("GetAvailableSectionForSelection");
    }

    public async Task<IActionResult> UnselectSection(int studentId, int sectionId)
    {
        var result = await selectionService.UnselectSection(studentId, sectionId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("GetAvailableSectionForSelection");
    }

}
