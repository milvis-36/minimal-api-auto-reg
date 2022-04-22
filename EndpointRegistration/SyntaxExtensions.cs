using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EndpointRegistration;
internal static class SyntaxExtensions
{
	public static bool IsClassImplementingInterface(this SyntaxNode syntaxNode, string interfaceName) =>
		syntaxNode is ClassDeclarationSyntax classDeclarationSyntax &&
		IsClassImplementingInterface(classDeclarationSyntax, interfaceName);

	public static bool IsClassImplementingInterface(this ClassDeclarationSyntax classDeclarationSyntax, string interfaceName) =>
		classDeclarationSyntax.BaseList?.Types.Any(t => t.Type.ToString() == interfaceName) == true;

	public static string GetIdentifier(this ClassDeclarationSyntax cls) => cls.Identifier.ToString();

	public static PropertyDeclarationSyntax GetPropertyByIdentifier(this ClassDeclarationSyntax cls, string propertyName) =>
		cls.Members.FirstOrDefault(m =>
			(m as PropertyDeclarationSyntax)?.Identifier.ToString() == propertyName) as PropertyDeclarationSyntax;

	public static MethodDeclarationSyntax GetMethodByIdentifier(this ClassDeclarationSyntax cls, string methodName) =>
		cls.Members.FirstOrDefault(m =>
			(m as MethodDeclarationSyntax)?.Identifier.ToString() == methodName) as MethodDeclarationSyntax;

	public static bool IsPartial(this ClassDeclarationSyntax cls) => cls.Modifiers.IsPartial();
	public static bool IsPartial(this MethodDeclarationSyntax prop) => prop?.Modifiers.IsPartial() == true;

	public static bool IsPartial(this SyntaxTokenList modifiers) =>
		modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
}
