using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Term;
using Application.Interfaces;
using Base;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Shared.Constants;


public class TermController : BaseController {

    private readonly ITermService _termService;

    public TermController(ITermService termService, IFacultyService facultyService) : base(facultyService)
    {
        _termService = termService;
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Index()
    {
        var model = await _termService.GetCurrentTerm();

        return View(model);
    }


    // list all the terms 
    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Terms()
    {
        var model = await _termService.GetAllTerms();

        return View(model);
    }

    // open new term
    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Add()
    {
        // Make it view component 
        // var model = await _termService.GetLastPreviousTerm();
        var model = await _termService.IsInsideAnyTermCurrently();

        if (model){
            ShowMessage("Currently a term already open", false);

            return RedirectToAction("Index");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Add(OpenNewTermDto model)
    {
        if (!ModelState.IsValid){
            return View(model);
        }

        var result = await _termService.OpenNewTerm(model);


        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("Index");
        }

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Edit()
    {
        var model = await _termService.GetCurrentTerm();

        if (model == null){
            ShowMessage("No term found", false);

            return RedirectToAction("Index");
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Edit(TermDto model)
    {
        var result = await _termService.EditTerm(model);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("Index");
        }

        return View(model);
    }


    [HttpGet]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Close()
    {
        var model = await _termService.GetCurrentTerm();

        if (model == null){
            ShowMessage("There is no term opened right now", false);

            return RedirectToAction("Add");
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Admin)]
    public async Task<IActionResult> Close(string confirmation, string currentTerm)
    {
        var result = await _termService.CloseTerm(confirmation, currentTerm);
        ShowMessage(result.Message, result.Succeeded);

        return RedirectToAction("Index");
    }

}
