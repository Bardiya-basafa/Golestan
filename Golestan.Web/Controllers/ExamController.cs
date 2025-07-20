namespace Golestan.Web.Controllers;

using Application.DTOs.Objection;
using Application.DTOs.Score;
using Application.Interfaces;
using Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;


public class ExamController(IExamService examService, IFacultyService facultyService) : BaseController(facultyService) {

    [HttpGet]
    [Authorize(Roles = AppRoles.Instructor)]
    public async Task<IActionResult> Index(int sectionId)
    {
        var userId = GetUserId();
        var model = await examService.GetSectionExamResults(sectionId, userId);
        ViewBag.SectionId = sectionId;

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> Results(int termId)
    {
        var userId = GetUserId();
        var model = await examService.GetTermExamResults(termId, userId);
        ViewBag.TermId = termId;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Instructor)]
    public async Task<IActionResult> SubmitStudentsScores(ScoreDto model)
    {
        var userId = GetUserId();
        var result = await examService.SubmitStudentScore(model, userId);

        ShowMessage(result.Message, result.Succeeded);


        return RedirectToAction("Index", new { sectionId = model.SectionId });
    }

    [HttpGet]
    [Authorize(Roles = AppRoles.Instructor)]
    public async Task<IActionResult> FinalResults( int termId)
    {
        var userId = GetUserId();
        var model = await examService.GetTermFinalResults(termId, userId);
        ViewBag.TermId = termId;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = AppRoles.Student)]
    public async Task<IActionResult> Objection(ObjectionDto dto)
    {
        var userId = GetUserId();
        var model = await examService.SubmitObjection(dto, userId);

        return RedirectToAction("Results", new { termId = dto.TermId});
    }

}
