namespace EndpointRegistration.Strategies.Common;

internal abstract class ClassDefinitionFinderBase : IEndpointFinder
{
	private static readonly NamespaceResolver NamespaceResolver = new();

	public IEndpointFinder? Next { get; }

	protected ClassDefinitionFinderBase(IEndpointFinder? next = null)
	{
		Next = next;
	}

	public EndpointDefinition? Resolve(SyntaxNode syntaxNode)
		=> ResolveInternal(syntaxNode) ?? Next?.Resolve(syntaxNode);


	private EndpointDefinition? ResolveInternal(SyntaxNode syntaxNode)
	{
		if (syntaxNode is not ClassDeclarationSyntax cls || !IsEndpointDefinition(cls))
		{
			return null;
		}

		ValidateMandatoryStructure(cls);

		return ResolveClassDeclarationSyntax(cls);
	}

	protected abstract bool IsEndpointDefinition(ClassDeclarationSyntax cls);

	protected abstract void ValidateMandatoryStructure(ClassDeclarationSyntax cls);

	protected abstract EndpointDefinition? ResolveClassDeclarationSyntax(ClassDeclarationSyntax cls);

	protected virtual string ResolveNamespace(ClassDeclarationSyntax cls) => NamespaceResolver.GetNamespace(cls);

	


}
