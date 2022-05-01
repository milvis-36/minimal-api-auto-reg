namespace EndpointRegistration.Strategies.Common;

internal class IgnoreEndpointFinder : IEndpointFinder
{
	public IEndpointFinder? Next { get; }

	public IgnoreEndpointFinder(IEndpointFinder? next = null)
	{
		Next = next;
	}

	public EndpointDefinition? Resolve(SyntaxNode syntaxNode)
	{
		if (syntaxNode is ClassDeclarationSyntax cls &&
				cls.AttributeLists.TryGetAttribute(IgnoreEndpointAttribute.AttributeName) is not null)
		{
			return null;
		}

		return Next?.Resolve(syntaxNode);
	}

}
