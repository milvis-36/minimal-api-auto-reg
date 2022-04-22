using EndpointRegistration;

namespace MinimalApiSourceGenerator.Endpoints.Products;
	public class ListApiEndpoint : IApiEndpoint
	{
		public string Pattern => "/home/index";
		public Delegate Handler { get; } = (HttpContext c) => { return $"ahoj from {nameof(ListApiEndpoint)}: {c.Request.Path}"; };
}

