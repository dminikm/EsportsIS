using BusinessLayer;
using Microsoft.AspNetCore.Http;
using System.Dynamic;

class BaseController : Controller
{
    public override void OnBeforeReply(HttpContext context)
    {
        base.OnBeforeReply(context);

        var session = context.Session;
        var sessionID = session.GetString("SESSION_ID");

        if (sessionID != null)
        {
            dynamic s = SessionManager.GetSession(sessionID);

            ViewBag.LoggedIn = LoggedIn = true;
            ViewBag.User = LoggedUser = (User)s.User;
        }
        else
        {
            ViewBag.LoggedIn = LoggedIn = false;
        }
    }

    protected bool LoggedIn { get; set; }
    protected User LoggedUser { get; set; }
}