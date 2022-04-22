
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EndpointRegistration.Strategies;

internal class InterfaceEndpointDefFinder : ClassDefinitionSyntaxFinderBase
{
	private static readonly ICollection<string> EndpointInterfaces = new List<string>
	{
		nameof(IApiEndpoint),
		nameof(IApiGetEndpoint),
		nameof(IApiPutEndpoint),
		nameof(IApiPostEndpoint),
		nameof(IApiDeleteEndpoint)
	};

	public InterfaceEndpointDefFinder(IEndpointFinder? next = null) : base(next)
	{ }

	protected override EndpointDefinition? ResolveClassDeclarationSyntax(ClassDeclarationSyntax cls)
	{
		if (!EndpointInterfaces.Any(cls.IsClassImplementingInterface))
		{
			return null;
		}

		return new EndpointDefinition
		{
			Pattern = PatternProperty ?? throw new GeneratorException(),
			ClassName = cls.GetIdentifier(),
			Namespace = Namespace,
			HttpMethod = ResolveHttpMethod(cls) ?? throw new GeneratorException(),
		};
	}

	private static string? ResolveHttpMethod(ClassDeclarationSyntax cls)
	{
		foreach (var ( interfaceName, _, method ) in Settings.Mappings)
		{
			if (cls.IsClassImplementingInterface(interfaceName))
			{
				return method;
			}
		}

		return null;
	}
}
