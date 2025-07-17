namespace Golestan.Web.Components;

using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


public class StudentInfoViewComponent(IStudentService studentService) : ViewComponent {

    public async Task<IViewComponentResult> InvokeAsync(int studentId)
    {
        var model = await studentService.GetStudentInfo(studentId);

        return View(model);
    }

}
