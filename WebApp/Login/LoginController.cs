class LoginController : BaseController
{
    public LoginController() : base()
    {

    }

    public ControllerAction Index()
    {
        return View<TestView, int>(0);
    }

    public ControllerAction Login(string username, string password)
    {
        return Redirect("/");
    }
}