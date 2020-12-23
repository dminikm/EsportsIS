using System.Dynamic;

class LoginIndexView : View
{
    public LoginIndexView() : base()
    {

    }

    public override string Render()
    {
        // DEBUG: Remove username and password
        return new Layout(ViewBag).Render($@"
<div class=""login-container"">
    <form action=""/login/"" method=""POST"">
        <h1>LogIn</h1>
        <input type=""text"" placeholder=""Username"" required minlength=""4"" maxlength=""9"" name=""username"">
        <input type=""password"" placeholder=""Password"" required minlength=""2"" maxlength=""10"" name=""password"">
        <input type=""submit"" value=""Submit"">
    </form>
</div>
        ");
    }
}