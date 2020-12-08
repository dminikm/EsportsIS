class LoginController : Controller
{
    public LoginController() : base()
    {

    }

    public ControllerAction Index(int a)
    {
        return View<LoginIndexView>();
    }
}