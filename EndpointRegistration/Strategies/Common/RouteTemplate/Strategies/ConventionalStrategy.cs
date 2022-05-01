using EndpointRegistration.Strategies.Common.RouteTemplate.Strategies.RouteParams;

namespace EndpointRegistration.Strategies.Common.RouteTemplate.Strategies;

internal class ConventionalStrategy : StrategyChainingBase
{
	private static readonly RouteParamsResolver RouteParamsResolver = new ();

	public ConventionalStrategy(IRouteTemplateFinder? next = null) : base(next)
	{ }

	protected override string TryFindRouteParamInternal(ClassDeclarationSyntax cls, string endpointName)
	{
		var parameters = FindHandlerParameterList(cls);
		var routeParameters = RouteParamsResolver.GetRouteParams(parameters);
		var patternBase = BuildPattern(endpointName, routeParameters);

		return patternBase;
	}

	private static string BuildPattern(string endpointName, params string[]? parameters)
	{
		var end = endpointName.Length;
		var pattern = new StringBuilder(end + 5);
		for (var i = 0; i < end; i++)
		{
			var c = endpointName[i];
			if (char.IsUpper(c))
			{
				pattern.Append("/");
			}
			pattern.Append(char.ToLowerInvariant(c));
		}

		parameters ??= Array.Empty<string>();
		foreach (var parameter in parameters)
		{
			pattern.Append("/");
			pattern.Append($"{{{parameter}}}");
		}

		return $"\"{pattern}\"";
	}
	
	private static ParameterListSyntax? FindHandlerParameterList(ClassDeclarationSyntax cls)
		=> TryHandlerPropertyDeclaration(cls) ?? TryHandlerMethodDeclaration(cls);

	private static ParameterListSyntax? TryHandlerPropertyDeclaration(ClassDeclarationSyntax cls)
	{
		var handlerProperty = cls.GetPropertyByIdentifier(nameof(IApiEndpoint.Handler));
		var handlerParams = (handlerProperty?.ExpressionBody?.Expression as ParenthesizedLambdaExpressionSyntax)?.ParameterList;

		return handlerParams;
	}

	private static ParameterListSyntax? TryHandlerMethodDeclaration(ClassDeclarationSyntax cls)
	{
		var handlerMethod = cls.GetMethodByIdentifier(nameof(IApiEndpoint.Handler));
		var handlerParams = handlerMethod?.ParameterList;

		return handlerParams;
	}
}
