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

    public Router AddRoute<ControllerType>(HTTPMethod method, string fullRoute, string handlerName)
        where ControllerType : Controller, new()
    {
        var parts = fullRoute.Split('/').Filter((x) => x != "").ToList();
        var parameterParts = parts
            .Filter((x) => x.StartsWith('{'))
            .Map((x) => x.Split('{', '}')[1])
            .ToList();

        var handler = typeof(ControllerType).GetMethod(handlerName);
        var paramInfos = handler.GetParameters().ToList();
        paramInfos.Sort((x, y) => x.Position - y.Position);

        handlers.Add(new Route() {
            Method = method,
            FullPath = fullRoute,
            Parts = parts,
            Handler = (string url, HttpContext context) =>
            {
                var request = context.Request;
                var routeValues = request.RouteValues;

                // Create a new controller for this request
                var controller = new ControllerType().BindContext(context);
                controller.OnBeforeReply(context);

                var res = (ControllerAction)handler.Invoke(
                    controller,
                    paramInfos.Map(
                        (x) => Convert.ChangeType(
                            routeValues.ContainsKey(x.Name) ?
                                routeValues[x.Name] :
                                request.Method == "POST" && request.Form.ContainsKey(x.Name) ?
                                    request.Form[x.Name][0] :
                                    throw new ArgumentException($"No argument named {x.Name} supplied to the url!"),
                            x.ParameterType
                        )
                    ).ToArray()
                );

                res.Run(context);
                return true;
            }
        });

        return this;
    }

    public Router BindRoutes(IEndpointRouteBuilder builder)
    {
        foreach (var handler in handlers)
        {
            if (handler.Method == HTTPMethod.GET)
            {
                builder.MapGet(handler.FullPath, async (context) => { handler.Handler(context.Request.Path, context); });
            }
            else
            {
                builder.MapPost(handler.FullPath, async (context) => { handler.Handler(context.Request.Path, context); });
            }
        }

        return this;
    }

    private List<Route> handlers;
}