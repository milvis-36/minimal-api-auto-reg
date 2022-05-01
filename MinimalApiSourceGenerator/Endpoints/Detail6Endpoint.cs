using EndpointRegistration;

namespace MinimalApiSourceGenerator.Endpoints;

public class Detail6ApiEndpoint : IAutoRegisterApiEndpoint
{
	public RouteHandlerBuilder Register(IEndpointRouteBuilder app)
	{
		return app.MapGet("/ax", () => { }).Produces(200);
	}
}

public class Detail7AutoRegisterApiEndpoint
{
	public RouteHandlerBuilder Register(IEndpointRouteBuilder app) => app.MapGet("/bx", () => { }).Produces(200);
}

public static class Detail8ArApiEndpoint
{
	public static RouteHandlerBuilder Register(IEndpointRouteBuilder app)
	{
		return app.MapGet("/cx", () => { }).Produces(200);
	}
}
