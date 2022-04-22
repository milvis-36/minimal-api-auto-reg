using EndpointRegistration.Models;

namespace EndpointRegistration.Strategies;
internal interface IEndpointFinder
{
	IEndpointFinder? Next { get; }

	EndpointDefinition? Resolve(SyntaxNode syntaxNode);
}
