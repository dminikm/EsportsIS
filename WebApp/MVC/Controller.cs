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
    protected ControllerAction(Action<HttpContext> onBeforeRun)
    {
        this.onBeforeRun = onBeforeRun;
    }

    public async void Run(HttpContext context)
    {
        this.onBeforeRun(context);
        this.Do(context);
    }

    protected virtual async void Do(HttpContext context)
    {
        throw new InvalidOperationException("Attempted to call base ControllerAction Do!");
    }


    private Action<HttpContext> onBeforeRun;
}

class ControllerViewAction<ViewType, ModelType> : ControllerAction where ViewType : View<ModelType> 
{
    public ControllerViewAction(Action<HttpContext> onBeforeRun, ViewType view, ModelType model) : base(onBeforeRun)
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
    public ControllerViewAction(Action<HttpContext> onBeforeRun, ViewType view) : base(onBeforeRun)
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
    public ControllerRedirectAction(Action<HttpContext> onBeforeRun, string url) : base(onBeforeRun)
    {
        this.redirectURL = url;
    }

    protected override async void Do(HttpContext context)
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
            (context) => this.OnBeforeReply(context),
            new ViewType()
        );
    }

    protected ControllerAction Redirect(string url)
    {
        return new ControllerRedirectAction(
            (context) => this.OnBeforeReply(context),
            url
        );
    }

    public dynamic ViewBag { get; set; }
}