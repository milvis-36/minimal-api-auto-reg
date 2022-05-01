namespace EndpointRegistration.Strategies.Common.RouteTemplate.Strategies;

internal abstract class StrategyChainingBase : IRouteTemplateFinder
{
	protected StrategyChainingBase(IRouteTemplateFinder? next = null)
	{
		Next = next;
	}

	public IRouteTemplateFinder? Next { get; }

	public string? TryFindRouteTemplate(ClassDeclarationSyntax cls, string endpointName)
		=> TryFindRouteParamInternal(cls, endpointName) ?? Next?.TryFindRouteTemplate(cls, endpointName);

	protected abstract string? TryFindRouteParamInternal(ClassDeclarationSyntax cls, string endpointName);
}
