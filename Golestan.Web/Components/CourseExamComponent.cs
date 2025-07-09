namespace Golestan.Web.Components;

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class CourseExamComponent : ViewComponent {

    private readonly ICourseService _courseService;

    public CourseExamComponent(ICourseService courseService)
    {
        _courseService = courseService;
    }

    // public async Task<IViewComponentResult> InvokeAsync()
    // {
    // }

}
