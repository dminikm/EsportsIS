using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;

enum HTTPMethod
{
    GET,
    POST,
}

struct Route
{
    public HTTPMethod Method { get; set; }
    public string FullPath { get; set; }
    public List<string> Parts { get; set; }
    public Func<string, HttpContext, bool> Handler { get; set; }
}

class Router
{
    public Router()
    {
        this.handlers = new List<Route>();
    }

    public Router AddRoute<HandlerType>(HTTPMethod method, string fullRoute, HandlerType handler)
        where HandlerType : Delegate
    {
        var parts = fullRoute.Split('/').Filter((x) => x != "").ToList();
        var parameterParts = parts
            .Filter((x) => x.StartsWith('{'))
            .Map((x) => x.Split('{', '}')[1])
            .ToList();

        var type = handler.GetType();
        var paramInfos = handler.Method.GetParameters().ToList();
        paramInfos.Sort((x, y) => x.Position - y.Position);

        handlers.Add(new Route() {
            Method = method,
            FullPath = fullRoute,
            Parts = parts,
            Handler = (string url, HttpContext context) =>
            {
                var res = (ControllerAction)handler.DynamicInvoke(
                    paramInfos.Map(
                        (x) => Convert.ChangeType(context.Request.RouteValues[x.Name], x.ParameterType)
                    ).ToArray()
                );

                res.Do(context);
                return true;
            }
        });

        return this;
    }

    public Router BindRoutes(IEndpointRouteBuilder builder)
    {
        foreach (var handler in handlers)
        {
            builder.MapGet(handler.FullPath, async (context) => { handler.Handler(context.Request.Path, context); });
        }

        return this;
    }

    private List<Route> handlers;
}