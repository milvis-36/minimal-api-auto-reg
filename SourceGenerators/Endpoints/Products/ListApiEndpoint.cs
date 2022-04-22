using Microsoft.AspNetCore.Http;

namespace SourceGenerators.Endpoints.Products
{

	public class ListApiEndpoint : IApiEndpoint
	{
		public string Path => "aaa";
		public RequestDelegate RequestDelegate { get; }
	}
}
