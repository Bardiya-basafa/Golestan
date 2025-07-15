namespace Golestan.Web.Controllers;

using Application.DTOs.Score;
using Application.Interfaces;
using Base;
using Microsoft.AspNetCore.Mvc;


public class ExamController(IExamService examService, IFacultyService facultyService) : BaseController(facultyService) {

    [HttpGet]
    public async Task<IActionResult> Index(int sectionId)
    {
        var model = await examService.GetSectionExamResults(sectionId);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitStudentsScores(ScoreDto model)
    {
        var result = await examService.SubmitStudentScore(model);

        if (!result.Succeeded){
            ShowMessage(result.Message, result.Succeeded);
        }

        return RedirectToAction("SubmitStudentsScores", new { instructorId = model.InstructorId, sectionId = model.SectionId });
    }

}
