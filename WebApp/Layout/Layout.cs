using System.Dynamic;

class Layout : View<string>
{
    public Layout(ExpandoObject obj) : base()
    {
        this.ViewBag = obj;
    }

    private string RenderUser()
    {
        return $@"
{(ViewBag.LoggedIn ?
    @$"<a>{ViewBag.User.FirstName} {ViewBag.User.LastName}</a>" :
    @$"<a href=""/login"">LogIn</a>"
)}
        ";
    }

    public string Render(string inner)
    {
        return $@"
<!DOCTYPE html>
<html>
    <head>
        <link rel=""stylesheet"" href=""/css/main.css"">
    </head>

    <body>
        <nav class=""navbar"">
            <a href=""/"" class=""navbar-company-name"">Esport IS</a>
            <div class=""navbar-user-container"">
                {RenderUser()}
            </div>
        </nav>
        <main class=""content-container"">
            {inner}
        </main>
    </body>
</html>           
        ";
    }
}