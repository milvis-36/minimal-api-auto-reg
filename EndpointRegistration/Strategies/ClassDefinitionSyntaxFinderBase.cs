using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EndpointRegistration.Strategies;


internal abstract class ClassDefinitionSyntaxFinderBase : IEndpointFinder
{
	public IEndpointFinder? Next { get; }

	protected ClassDefinitionSyntaxFinderBase(IEndpointFinder? next = null)
	{
		Next = next;
	}

	public EndpointDefinition? Resolve(SyntaxNode syntaxNode)
		=> ResolveInternal(syntaxNode) ?? Next?.Resolve(syntaxNode);

	protected string? PatternProperty { get; private set; }

	protected string Namespace { get; private set; } = "generated";

	private EndpointDefinition? ResolveInternal(SyntaxNode syntaxNode)
	{
		if (syntaxNode is not ClassDeclarationSyntax cls)
		{
			return null;
		}

		PatternProperty = cls.GetPropertyByIdentifier(nameof(IApiEndpoint.Pattern)).ExpressionBody?.Expression.ToString();
		Namespace = (cls.Parent as BaseNamespaceDeclarationSyntax)?.Name.ToString() ?? throw new GeneratorException();
		
		return ResolveClassDeclarationSyntax(cls);
	}

	protected abstract EndpointDefinition? ResolveClassDeclarationSyntax(ClassDeclarationSyntax cls);
}
