namespace EndpointRegistration.Strategies.Common;

internal class NamespaceResolver
{
	public string GetNamespace(ClassDeclarationSyntax cls)
		=> (cls.Parent as BaseNamespaceDeclarationSyntax)?.Name.ToString() ?? throw new GeneratorException($"{nameof(NamespaceResolver)}: Failed resolve namespace on class '{cls.GetIdentifier()}'");
}
