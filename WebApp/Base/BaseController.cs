using Microsoft.AspNetCore.Http;
using System.Dynamic;

class BaseController : Controller
{
    public override void OnBeforeReply(HttpContext context)
    {
        base.OnBeforeReply(context);

        ViewBag.LoggedIn = false;
    }
}