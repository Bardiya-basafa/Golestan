namespace Golestan.Web.Controllers;

using Application.DTOs.Objection;
using Application.DTOs.Score;
using Application.Interfaces;
using Base;
using Microsoft.AspNetCore.Mvc;


public class ExamController(IExamService examService, IFacultyService facultyService) : BaseController(facultyService) {

    [HttpGet]
    public async Task<IActionResult> Index(int sectionId)
    {
        var model = await examService.GetSectionExamResults(sectionId);
        ViewBag.SectionId = sectionId;

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Results(int studentId, int termId)
    {
        var model = await examService.GetTermExamResults(termId, studentId);
        ViewBag.StudentId = studentId;
        ViewBag.TermId = termId;

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitStudentsScores(ScoreDto model)
    {
        var result = await examService.SubmitStudentScore(model);

        ShowMessage(result.Message, result.Succeeded);


        return RedirectToAction("Index", new { sectionId = model.SectionId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Objection(ObjectionDto dto)
    {
        var model = await examService.SubmitObjection(dto);

        return View(model);
    }

}
