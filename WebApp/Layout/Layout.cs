using System.Dynamic;

class Layout : View<string>
{
    public Layout(ExpandoObject obj) : base()
    {
        this.ViewBag = obj;
    }

    private string RenderUser()
    {
        if (ViewBag.LoggedIn)
        {
            return $@"
                <div>{ViewBag.User.FirstName} {ViewBag.User.LastName}</div>
                <a href=""/logout"">LogOut</a>
            ";
        }

        return $@"<a href=""/login"">LogIn</a>";
    }

    public override string Render(string inner)
    {
        return $@"
<!DOCTYPE html>
<html>
    <head>
        <meta charset=""utf-8"" />
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