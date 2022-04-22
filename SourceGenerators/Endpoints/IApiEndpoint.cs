using Microsoft.AspNetCore.Http;


namespace SourceGenerators.Endpoints;

public interface IApiEndpoint
{
	string Path { get; }

	RequestDelegate RequestDelegate { get; }
}
