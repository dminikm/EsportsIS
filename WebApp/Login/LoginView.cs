class LoginIndexView : View
{
    public string Render()
    {
        return "Hello, world!";
    }
}

class TestView : View<int>
{
    public string Render(int a)
    {
        return $"Hello {a}";
    }
}