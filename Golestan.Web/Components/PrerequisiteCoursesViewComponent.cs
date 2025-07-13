namespace Golestan.Web.Components;

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class PrerequisiteCoursesViewComponent : ViewComponent {

    private readonly ICourseService _courseService;

    public PrerequisiteCoursesViewComponent(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<IViewComponentResult> InvokeAsync(int courseId)
    {
        var model = await _courseService.GetPrerequisiteCourses(courseId);

        return View(model);
    }

}
