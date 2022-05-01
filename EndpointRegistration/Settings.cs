namespace EndpointRegistration;

internal static class Settings
{
	private static class HttpMethods
	{
		public const string Get = "Get";
		public const string Post = "Post";
		public const string Put = "Put";
		public const string Delete = "Delete";
	}

	internal static readonly List<EndpointMappingInfo> Mappings = new()
	{
		new EndpointMappingInfo(nameof(IApiGetEndpoint), $"{HttpMethods.Get}{Constants.EndpointSuffix}", HttpMethods.Get),
		new EndpointMappingInfo(nameof(IApiPostEndpoint), $"{HttpMethods.Post}{Constants.EndpointSuffix}", HttpMethods.Post),
		new EndpointMappingInfo(nameof(IApiPutEndpoint), $"{HttpMethods.Put}{Constants.EndpointSuffix}", HttpMethods.Put),
		new EndpointMappingInfo(nameof(IApiDeleteEndpoint), $"{HttpMethods.Delete}{Constants.EndpointSuffix}", HttpMethods.Delete),
		new EndpointMappingInfo(nameof(IApiRouteEndpoint), Constants.EndpointSuffix, HttpMethods.Get),
		new EndpointMappingInfo(nameof(IApiEndpoint), Constants.EndpointSuffix, HttpMethods.Get),
	};
}

