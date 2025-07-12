namespace Golestan.Web.Components;

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class CourseExamViewComponent : ViewComponent {

    private readonly ICourseService _courseService;

    public CourseExamViewComponent(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int courseId)
    {
        var model = await _courseService.GetCourseExam(courseId);

        return View(model);
    }

}
