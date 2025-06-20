using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers;

using Application.DTOs.Term;
using Application.Interfaces;
using Base;
using Domain.Entities;


public class TermController : BaseController {

    private readonly ITermService _termService;

    public TermController(ITermService termService)
    {
        _termService = termService;
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
        var model = await _termService.TermOpeningOptions();

        if (!model.CanOpenAnyTermNow){
            ShowMessage("Currently a term already open", false);

            return RedirectToAction("CurrentTerm");
        }

        return View(model);
    }

    // open normal term
    [HttpGet]
    public IActionResult OpenNormalTerm()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OpenNormalTerm(OpenNewTermDto dto)
    {
        var result = await _termService.OpenNormalTerm(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("CurrentTerm");
        }

        return View(result);
    }

    // open costume term 
    [HttpGet]
    public IActionResult OpenCostumeTerm()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OpenCostumeTerm(OpenNewTermDto dto)
    {
        var result = await _termService.OpenCostumeTerm(dto);
        ShowMessage(result.Message, result.Succeeded);

        if (result.Succeeded){
            return RedirectToAction("CurrentTerm");
        }

        return View(result);
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
