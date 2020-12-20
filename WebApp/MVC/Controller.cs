using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Dynamic;

class ControllerAction
{
    public void Run(HttpContext context)
    {
        this.Do(context);
    }

    protected virtual void Do(HttpContext context)
    {
        throw new InvalidOperationException("Attempted to call base ControllerAction Do!");
    }
}

class ControllerViewAction<ViewType, ModelType> : ControllerAction where ViewType : View<ModelType> 
{
    public ControllerViewAction(Action<HttpContext> onBeforeRun, ViewType view, ModelType model)
    {
        this.view = view;
        this.model = model;
    }

    protected override async void Do(HttpContext context)
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

    protected override async void Do(HttpContext context)
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

    protected override void Do(HttpContext context)
    {
        context.Response.Redirect(redirectURL);
    }

    private string redirectURL;
}

class Controller
{
    public Controller()
    {
        this.ViewBag = new ExpandoObject();
    }

    public Controller BindContext(HttpContext context)
    {
        this.context = context;
        
        return this;
    }

    public virtual void OnBeforeReply(HttpContext context)
    {
        
    }

    protected ControllerAction View<ViewType, ModelType>(ModelType model) where ViewType : View<ModelType>, new()
    {
        return new ControllerViewAction<ViewType, ModelType>(
            (context) => this.OnBeforeReply(context),
            new ViewType() { ViewBag = ViewBag },
            model
        );
    }

    protected ControllerAction View<ViewType>() where ViewType : View, new()
    {
        return new ControllerViewAction<ViewType>(
            new ViewType() { ViewBag = ViewBag }
        );
    }

    protected ControllerAction Redirect(string url)
    {
        return new ControllerRedirectAction(
            url
        );
    }

    public dynamic ViewBag { get; set; }
    public HttpContext context;
}