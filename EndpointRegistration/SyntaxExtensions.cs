namespace EndpointRegistration;
internal static class SyntaxExtensions
{
	public static bool IsClassImplementingInterface(this ClassDeclarationSyntax classDeclarationSyntax, string interfaceName) =>
		classDeclarationSyntax.BaseList?.Types.Any(t => t.Type.ToString() == interfaceName) == true;

	public static string GetIdentifier(this ClassDeclarationSyntax cls) => cls.Identifier.ToString() ?? throw new GeneratorException("Class identifier is null.");
	public static bool IdentifierEndsWith(this ClassDeclarationSyntax cls, string suffix) => cls.GetIdentifier().EndsWith(suffix);

	public static PropertyDeclarationSyntax? GetPropertyByIdentifier(this ClassDeclarationSyntax cls, string propertyName) =>
		cls.Members.FirstOrDefault(m =>
			(m as PropertyDeclarationSyntax)?.Identifier.ToString() == propertyName) as PropertyDeclarationSyntax;

	public static MethodDeclarationSyntax? GetMethodByIdentifier(this ClassDeclarationSyntax cls, string methodName) =>
		cls.Members.FirstOrDefault(m =>
			(m as MethodDeclarationSyntax)?.Identifier.ToString() == methodName) as MethodDeclarationSyntax;

	public static bool IsPartial(this SyntaxTokenList modifiers) =>
		modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));

	public static bool IsStatic(this SyntaxTokenList modifiers) =>
		modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword));

	public static AttributeSyntax? TryGetAttribute(this SyntaxList<AttributeListSyntax> al, string attributeName)
		=> al.SelectMany(x => x.Attributes)
			.LastOrDefault(x =>
				x.IsKind(SyntaxKind.Attribute) &&
				(attributeName.Equals(x.Name.ToString(), StringComparison.InvariantCultureIgnoreCase)
				|| x.Name.ToString().EndsWith(attributeName, StringComparison.InvariantCultureIgnoreCase) == true));

	public static string? TryGetAttributeValue(this AttributeSyntax? a, int index)
		=> a?.ArgumentList?.Arguments[index].ToString();

	public static string? TryGetPropertyValue(this ClassDeclarationSyntax cls, string propertyName)
	{
		var propertyDeclarationSyntax = cls.GetPropertyByIdentifier(propertyName);
		if (propertyDeclarationSyntax is null)
		{
			return null;
		}

		return
			// PropertyName => "PropertyValue"
			propertyDeclarationSyntax
				.ExpressionBody
				?.Expression
				.ToString()
			// PropertyName { get; } = "PropertyValue"
			?? propertyDeclarationSyntax
				.Initializer
				?.Value
				.ToString()
			// PropertyName { get => "PropertyValue" }
			?? propertyDeclarationSyntax
				.AccessorList?.Accessors
				.SingleOrDefault(x => x.IsKind(SyntaxKind.GetAccessorDeclaration))
				?.ExpressionBody
				?.Expression
				.ToString()
			// PropertyName { get { return "PropertyValue"; } }
			?? (propertyDeclarationSyntax
				.AccessorList
				?.Accessors
				.SingleOrDefault(x => x.IsKind(SyntaxKind.GetAccessorDeclaration))
				?.Body
				?.Statements
				.SingleOrDefault(x => x.IsKind(SyntaxKind.ReturnStatement)) as ReturnStatementSyntax)
			?.Expression
			?.ToString();
	}
}
