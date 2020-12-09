using Microsoft.AspNetCore.Http;
using System.Dynamic;

class BaseController : Controller
{
    public override void OnBeforeReply(HttpContext context)
    {
        ViewBag.LoggedIn = false;
    }
}