namespace EndpointRegistration.Strategies.Common.RouteTemplate.Strategies;

internal class PatternPropertyStrategy : StrategyChainingBase
{
	public PatternPropertyStrategy(IRouteTemplateFinder? next = null) : base(next)
	{ }

	protected override string? TryFindRouteParamInternal(ClassDeclarationSyntax cls, string _)
	{
		var pattern = cls.TryGetPropertyValue(nameof(IApiRouteEndpoint.Pattern));

		return pattern;
	}
}
