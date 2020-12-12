using Microsoft.AspNetCore.Http;
using System.Dynamic;
using BusinessLayer;

class LoginController : BaseController
{
    public ControllerAction Index()
    {
        if (LoggedIn)
            return Redirect("/overview");

        return Redirect("/login");
    }

    public ControllerAction LoginGET()
    {
        return View<LoginIndexView>();
    }

    private ExpandoObject GetLoginSession(User usr)
    {
        dynamic s = new ExpandoObject();
        s.User = usr;
        return s;
    }

    public ControllerAction LoginPOST(string username, string password)
    {
        var session = context.Session;
        
        var user = User.FindByUsernamePassword(username, password);
        return user.Match((usr) => {
            var s = GetLoginSession(usr);
            var sess = SessionManager.AddSession(s);
            context.Session.SetString("SESSION_ID", sess);

            return Redirect("/overview");
        }, () => {
            return Redirect("/");
        });
    }

    public ControllerAction Logout()
    {
        var session = context.Session;
        var sessionID = session.GetString("SESSION_ID");

        if (sessionID != null)
        {
            SessionManager.RemoveSession(sessionID);
            session.Remove("SESSION_ID");
        }

        return Redirect("/");
    }
}