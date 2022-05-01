namespace EndpointRegistration.Strategies.Common.RouteTemplate;

internal interface IRouteTemplateFinder
{
	public IRouteTemplateFinder? Next { get; }

	public string? TryFindRouteTemplate(ClassDeclarationSyntax cls, string endpointName);
}
