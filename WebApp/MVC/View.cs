using System;

class View<ModelType>
{
    public View()
    {

    }

    public virtual string Render(ModelType model)
    {
        throw new NotImplementedException("Base View has no Render");
    }

    public dynamic ViewBag { get; set; }
}

class View
{
    public View()
    {

    }

    public virtual string Render()
    {
        throw new NotImplementedException("Base View has no Render");
    }

    public dynamic ViewBag { get; set; }
}