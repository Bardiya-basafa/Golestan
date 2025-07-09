using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Term;
using Application.Interfaces;
using Base;
using Domain.Entities;


public class TermController : BaseController {

    private readonly ITermService _termService;

    public TermController(ITermService termService,IFacultyService facultyService) : base(facultyService)
    {
        _termService = termService;
    }

    [HttpGet]
    public async Task<IActionResult> TermManagement()
    {
        var model = await _termService.GetCurrentTerm();

        return View(model);
    }

    // display current term
    [HttpGet]
    public async Task<IActionResult> CurrentTerm()
    {
        var model = await _termService.GetCurrentTerm();

        return View(model);
    }

    // list all the terms 
    [HttpGet]
    public async Task<IActionResult> AllTerms()
    {
        var model = await _termService.GetAllTerms();

        return View();
    }

    // open new term
    [HttpGet]
    public async Task<IActionResult> OpenNewTerm()
    {
        // Make it view component 
        // var model = await _termService.GetLastPreviousTerm();
        var model = await _termService.IsInsideAnyTermCurrently();

        if (model){
            ShowMessage("Currently a term already open", false);

            return RedirectToAction("CurrentTerm");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OpenNewTerm(OpenNewTermDto dto)
    {
        if (!ModelState.IsValid){
            return View(dto);
        }

        var result = await _termService.OpenNewTerm(dto);


        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("CurrentTerm");
        }

        return View(dto);
    }


    // Edit current term
    // [HttpGet]
    // public async Task<IActionResult> EditTermProperties()
    // {
    //     var model = await _termService.GetCurrentTerm();
    //
    //     if (model == null){
    //         ShowMessage("Currently no term is available", false);
    //
    //         return RedirectToAction("OpenNewTerm");
    //     }
    //
    //     return View();
    // }
    //
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> EditTermProperties(EditTermDto dto)
    // {
    //     var result = await _termService.EditTermProperties();
    //     ShowMessage(result.Message, result.Succeeded);
    //
    //     if (result.Succeeded){
    //         return RedirectToAction("CurrentTerm");
    //     }
    //
    //     return View();
    // }

    [HttpGet]
    public async Task<IActionResult> CloseTerm()
    {
        var model = await _termService.GetCurrentTerm();

        if (model == null){
            ShowMessage("There is no term opened right now", false);

            return RedirectToAction("OpenNewTerm");
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CloseTerm(string confirmation, string currentTerm)
    {
        var result = await _termService.CloseTerm(confirmation, currentTerm);
        ShowMessage(result.Message, result.Succeeded);

        return View();
    }

}
