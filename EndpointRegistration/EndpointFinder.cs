using System.Diagnostics;
using EndpointRegistration.Models;
using EndpointRegistration.Strategies;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EndpointRegistration;
internal class EndpointFinder : ISyntaxReceiver
{
	private readonly IEndpointFinder _finder = new InterfaceEndpointDefFinder(new ConventionalEndpointDefFinder());

	public EndpointFinderResult Result { get; } = new();

	public void OnVisitSyntaxNode0(SyntaxNode syntaxNode)
	{
		var r = _finder.Resolve(syntaxNode);

	}

	public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
	{
		//if (!Debugger.IsAttached) Debugger.Launch();

		try
		{
			OnVisitSyntaxNode0(syntaxNode);
		}
		catch (GeneratorException e)
		{
			// TODO: result errors
			Console.WriteLine(e);
			throw;
		}
		


		ProcessIfImplements(syntaxNode, cls =>
		{
			var pathProperty = cls.GetPropertyByIdentifier(nameof(IApiEndpoint.Pattern));

			Result.EndpointDefinitions.Add(new EndpointDefinition
			{
				Pattern = Utils.IsConventionRegistration(syntaxNode) ? Utils.ResolvePattern(cls) : pathProperty?.ExpressionBody?.Expression.ToString(),
				ClassName = cls.Identifier.ToString(),
				Namespace = (cls.Parent as BaseNamespaceDeclarationSyntax)?.Name.ToString(),
				HttpMethod = Utils.ResolveHttpMethod(cls),
			});
		},
			nameof(IApiEndpoint),
			nameof(IApiGetEndpoint),
			nameof(IApiPutEndpoint),
			nameof(IApiPostEndpoint),
			nameof(IApiDeleteEndpoint));

		if (Utils.IsConventionRegistration(syntaxNode))
		{
			Console.WriteLine();
		}

	}

	private static void ProcessIfImplements(SyntaxNode syntaxNode,
		Action<ClassDeclarationSyntax> action, params string[] interfaceName)
	{
		//&& syntaxNode is ClassDeclarationSyntax cls && cls.Identifier.ToString().
		if (interfaceName?.Any(syntaxNode.IsClassImplementingInterface) != true
				&& !Utils.IsConventionRegistration(syntaxNode))
		{
			return;
		}

		action(syntaxNode as ClassDeclarationSyntax);
	}
}
