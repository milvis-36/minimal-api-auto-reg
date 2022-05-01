using EndpointRegistration.Strategies.Common.RouteTemplate.Strategies.RouteParams.Strategies;

namespace EndpointRegistration.Strategies.Common.RouteTemplate.Strategies.RouteParams;

internal class RouteParamsResolver
{
	private static readonly ICollection<IRouteParamsFinder> OrderedFinders = new List<IRouteParamsFinder>
	{
		new FromRouteAttributeStrategy(),
		new Strategies.ConventionalStrategy(),
	};

	public string[]? GetRouteParams(ParameterListSyntax? pl)
	{
		if (pl is null)
		{
			return null;
		}

		foreach (var routeParamsFinder in OrderedFinders)
		{
			var result = routeParamsFinder.GetRouteParams(pl);
			if (result is not null)
			{
				return result;
			}
		}

		return null;
	}
}
