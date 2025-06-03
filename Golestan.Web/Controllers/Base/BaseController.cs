using Microsoft.AspNetCore.Mvc;


namespace Golestan.Web.Controllers.Base;

public abstract class BaseController : Controller {

    public void ShowMessage(string? message, bool result)
    {
        TempData["NotificationMessage"] = message;
        TempData["Success"] = result;
    }

}
