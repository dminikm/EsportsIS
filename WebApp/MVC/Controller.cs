using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class ControllerAction
{
    public virtual async void Do(HttpContext context)
    {
        throw new InvalidOperationException("Attempted to call base ControllerAction Do!");
    }
}

class ControllerViewAction<ViewType, ModelType> : ControllerAction where ViewType : View<ModelType> 
{
    public ControllerViewAction(ViewType view, ModelType model)
    {
        this.view = view;
        this.model = model;
    }

    public override async void Do(HttpContext context)
    {
        await context.Response.WriteAsync(view.Render(model));
    }

    private ViewType view;
    private ModelType model;
}

class ControllerViewAction<ViewType> : ControllerAction where ViewType : View
{
    public ControllerViewAction(ViewType view)
    {
        this.view = view;
    }

    public override async void Do(HttpContext context)
    {
        await context.Response.WriteAsync(view.Render());
    }

    private ViewType view;
}

class ControllerRedirectAction : ControllerAction
{
    public ControllerRedirectAction(string url)
    {
        this.redirectURL = url;
    }

    public override async void Do(HttpContext context)
    {
        context.Response.Redirect(redirectURL);
    }

    private string redirectURL;
}

class Controller
{
    public Controller()
    {

    }


    protected ControllerAction View<ViewType, ModelType>(ModelType model) where ViewType : View<ModelType>, new()
    {
        return new ControllerViewAction<ViewType, ModelType>(new ViewType(), model);
    }

    protected ControllerAction View<ViewType>() where ViewType : View, new()
    {
        return new ControllerViewAction<ViewType>(new ViewType());
    }

    protected ControllerAction Redirect(string url)
    {
        return new ControllerRedirectAction(url);
    }
}