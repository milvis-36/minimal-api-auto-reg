using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApiSourceGenerator.Endpoints;

public class Detail1GetEndpoint
{
	// (cls.Members[0] as PropertyDeclarationSyntax).ExpressionBody.Expression.ToString() ---------
	public string Pattern0 => "/detail/{id}";

	// ((((cls.Members[1] as PropertyDeclarationSyntax).AccessorList.Accessors[0] as AccessorDeclarationSyntax).Body as BlockSyntax).Statements[0] as ReturnStatementSyntax).Expression.ToString()
	public string Pattern1
	{
		get
		{
			return "/detail/{id}";
		}
	}

	// (cls.Members[2] as PropertyDeclarationSyntax).AccessorList.Accessors[0].ExpressionBody.Expression.ToString() ---------
	public string Pattern2
	{
		get => "/detail/{id}";

	}

	// (cls.Members[3] as PropertyDeclarationSyntax).Initializer.Value.ToString() -----------
	public string Pattern3 { get; } = "/detail/{id}";

	public Delegate Handler0 => (int id) => $"id: {id}";
	public Delegate Handler => (int id, object o, MyClass c, [FromRoute] int r, [FromRoute(Name = "param-s")] int s) => $"id: {id}";

	[Route("aaaaaaa/aaa")]
	public string Handler2(int id)
	{
		return $"id: {id}";
	}
}

class MyClass
{
	
}