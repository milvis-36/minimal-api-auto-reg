namespace EndpointRegistration.Strategies.Common.RouteTemplate.Strategies;

internal class RouteAttributeStrategy : StrategyChainingBase
{
	private const string RouteAttributeName = "Route";

	public RouteAttributeStrategy(IRouteTemplateFinder? next = null) : base(next)
	{ }
	
	protected override string? TryFindRouteParamInternal(ClassDeclarationSyntax cls, string _)
	{
		var handlerMethod = cls.GetMethodByIdentifier(nameof(IApiEndpoint.Handler));
		var routeAttribute = handlerMethod?.AttributeLists.TryGetAttribute(RouteAttributeName);
		var template = routeAttribute.TryGetAttributeValue(0);

		return template;
	}
}
