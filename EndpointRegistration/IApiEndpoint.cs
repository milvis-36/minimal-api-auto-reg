namespace EndpointRegistration;

public interface IApiEndpoint
{
	public string Pattern { get; }

	public Delegate Handler { get; }
}

public interface IApiGetEndpoint : IApiEndpoint { }
public interface IApiPutEndpoint : IApiEndpoint { }
public interface IApiPostEndpoint : IApiEndpoint { }
public interface IApiDeleteEndpoint : IApiEndpoint { }
