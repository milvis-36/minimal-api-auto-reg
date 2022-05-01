using Microsoft.CodeAnalysis.CSharp;

namespace EndpointRegistration.Utils;

internal class PropertyValueReader
{
	public string? TryGetPropertyValue(ClassDeclarationSyntax cls, string propertyName)
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
