namespace Golestan.Web.Controllers;

using Application.Interfaces;
using Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using ISelectionService=Application.Interfaces.ISelectionService;


public class SelectionController : BaseController {

    private readonly ISelectionService _selectionService;

    private readonly ITermService _termService;

    public SelectionController(ISelectionService selectionService, IFacultyService facultyService, ITermService termService) : base(facultyService)
    {
        _selectionService = selectionService;
        _termService = termService;
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var currentTerm = await _termService.GetCurrentTerm();

        if (currentTerm == null){
            ShowMessage("No term available", false);

            return RedirectToAction("Selection", "Students");
        }

        var currentDate = DateTime.UtcNow;

        if (currentTerm.SelectionStartTime > currentDate || currentTerm.SelectionEndTime < currentDate){
            ShowMessage("Currently we are not at selection time", false);

            return RedirectToAction("Selection", "Students");
        }

        var model = await _selectionService.GetSelectionDto(userId);
        model.StartDate = currentTerm.SelectionStartTime;
        model.EndDate = currentTerm.SelectionEndTime;
        model.Term = currentTerm.TermIdentifier;

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> GetAvailableSectionForSelection()
    {
        var userId = GetUserId();
        var model = await _selectionService.GetAvailableSectionsForSelection(userId);

        if (model == null){
            ShowMessage("Right now selection is not available", false);

            return RedirectToAction("Index", "Students");
        }

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> SelectSection(int sectionId)
    {
        var userId = GetUserId();
        var result = await _selectionService.SelectSection(userId, sectionId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Index", "Selection");
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> UnselectSection(int sectionId)
    {
        var userId = GetUserId();
        var result = await _selectionService.UnselectSection(userId, sectionId);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Index", "Selection");
    }

}
