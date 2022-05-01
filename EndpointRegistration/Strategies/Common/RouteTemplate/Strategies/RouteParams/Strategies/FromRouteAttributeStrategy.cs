using Microsoft.CodeAnalysis.CSharp;

namespace EndpointRegistration.Strategies.Common.RouteTemplate.Strategies.RouteParams.Strategies;

internal class FromRouteAttributeStrategy : IRouteParamsFinder
{
	private const string FromRouteAttribute = "FromRoute";

	public string[]? GetRouteParams(ParameterListSyntax parameterListSyntax)
	{
		var routeParams =
			parameterListSyntax.Parameters.Where(p => p.AttributeLists.TryGetAttribute(FromRouteAttribute) is not null);
		var parameters = routeParams.Select(GetParamName).ToArray();
		if (parameters.Length == 0)
		{
			return null;
		}

		return parameters;
	}

	private static string GetParamName(ParameterSyntax p)
	{
		var nameParameter =
			(p.AttributeLists
				.TryGetAttribute(FromRouteAttribute)
				?.ArgumentList
				?.Arguments
				.SingleOrDefault(a => a.NameEquals.IsKind(SyntaxKind.NameEquals))
				?.Expression as LiteralExpressionSyntax)
				?.Token
				.ValueText;

		return nameParameter ?? p.Identifier.ValueText;
	}
}