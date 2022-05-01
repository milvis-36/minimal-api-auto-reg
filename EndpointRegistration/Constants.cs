// ReSharper disable InconsistentNaming
namespace EndpointRegistration;

internal class Constants
{
	public const string EndpointSuffix = "Endpoint";
	public static readonly List<string> AutoRegisterEndpointSuffixes = new(){"AutoRegisterApiEndpoint", "ArApiEndpoint"};
	public const string AutoRegisterMethodName= "Register";
	public const string AutoRegisterMethodReturnType = "RouteHandlerBuilder";
	public const string RouteHandlerBuilder = "RouteHandlerBuilder";
	public const string IEndpointRouteBuilder = "IEndpointRouteBuilder";
	public const string DefaultNamespace = "Generated";
	public const string RegisterClassName = "Endpoints";
	public const string ResultClassName = "Registrations";
	public const string ConfigureMethodName = "Configure";
	public const string RegisterMethodName = "AutoRegister";
	public const string WebApplicationParamName = "app";
	public const string GeneratedFileName = $"{RegisterClassName}_{RegisterMethodName}";
}
