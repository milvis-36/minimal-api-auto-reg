using EndpointRegistration.Strategies;
using EndpointRegistration.Strategies.AutoRegistering;
using EndpointRegistration.Strategies.Common;
using EndpointRegistration.Strategies.Conventional;
using EndpointRegistration.Strategies.InterfaceBased;

namespace EndpointRegistration;

internal class EndpointFinder : ISyntaxReceiver
{
	private readonly IEndpointFinder _finder =
		new IgnoreEndpointFinder(
		new AutoRegisterFinder(
			new InterfaceEndpointFinder(
				new ConventionalEndpointFinder())));

	public EndpointFinderResult Result { get; } = new();

	public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
	{
		try
		{
			var endpointDefinition = _finder.Resolve(syntaxNode);
			if (endpointDefinition is not null)
			{
				Result.EndpointDefinitions.Add(endpointDefinition);
			}
		}
		catch (GeneratorException e)
		{
			Result.GeneratorExceptions.Add(e);
		}
	}
}
