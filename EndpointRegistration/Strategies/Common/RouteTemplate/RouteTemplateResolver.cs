using EndpointRegistration.Strategies.Common.RouteTemplate.Strategies;

namespace EndpointRegistration.Strategies.Common.RouteTemplate;

internal class RouteTemplateResolver
{
	private static readonly IRouteTemplateFinder RouteParamFinder =
		new PatternPropertyStrategy(
			new RouteAttributeStrategy(
				new ConventionalStrategy()
				)
			);
	public string GetRoutePattern(ClassDeclarationSyntax cls, string endpointName)
	{
		string? routeParam = RouteParamFinder.TryFindRouteTemplate(cls, endpointName);

		return routeParam ?? throw new GeneratorException($"{nameof(RouteTemplateResolver)}: Unable to resolve endpoint's route pattern.");
	}
}
