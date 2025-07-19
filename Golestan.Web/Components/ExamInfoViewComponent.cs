namespace Golestan.Web.Components;

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class ExamInfoViewComponent(IExamService examService) : ViewComponent {

    public async Task<IViewComponentResult> InvokeAsync(int sectionId)

    {
        var model = await examService.GetExamInfo(sectionId);

        return View(model);
    }

}
