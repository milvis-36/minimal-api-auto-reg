using EndpointRegistration;

namespace MinimalApiSourceGenerator.Endpoints.Products;
public class ListApiEndpoint : IApiEndpoint
{
	public void Configure(RouteHandlerBuilder eb)
	{
		eb.Produces(200);

	}

	public string Pattern => "/home/index";
	public Delegate Handler { get; } = (HttpContext c) => { return $"ahoj from {nameof(ListApiEndpoint)}: {c.Request.Path}"; };
}

