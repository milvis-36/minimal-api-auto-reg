namespace EndpointRegistration;

public interface IRoutePattern
{
	public string Pattern { get; }
}


public interface IAutoRegisterApiEndpoint
{
	// TODO: Types not in .net standard 2.0 so far supported by source code generators
	//public RouteHandlerBuilder Register(IEndpointRouteBuilder app);
}

public interface IApiEndpoint
{
	// TODO: RouteHandlerBuilder not in .net standard 2.0 so far supported by source code generators
	//public void Configure(RouteHandlerBuilder mapped);

	public Delegate Handler { get; }
}

public interface IApiRouteEndpoint : IApiEndpoint, IRoutePattern { }

public interface IApiGetEndpoint : IApiEndpoint { }

public interface IApiPutEndpoint : IApiEndpoint { }

public interface IApiPostEndpoint : IApiEndpoint { }

public interface IApiDeleteEndpoint : IApiEndpoint { }
