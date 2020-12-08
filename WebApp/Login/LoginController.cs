class LoginController : Controller
{
    public LoginController() : base()
    {

    }

    public ControllerAction Index(int a)
    {
        return View<TestView, int>(a);
    }

    public ControllerAction Login(string name, string password)
    {
        return Redirect("/1");
    }
}