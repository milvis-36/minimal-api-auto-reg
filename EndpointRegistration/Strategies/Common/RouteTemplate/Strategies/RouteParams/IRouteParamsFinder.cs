namespace EndpointRegistration.Strategies.Common.RouteTemplate.Strategies.RouteParams;
internal interface IRouteParamsFinder
{
	string[]? GetRouteParams(ParameterListSyntax parameterListSyntax);
}
