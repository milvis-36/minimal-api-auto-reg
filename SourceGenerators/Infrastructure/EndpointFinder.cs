using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SourceGenerators.Endpoints;

namespace SourceGenerators.Infrastructure
{
	public class EndpointFinder : ISyntaxReceiver
	{
		public ICollection<ClassDeclarationSyntax> Endpoints { get; } = new List<ClassDeclarationSyntax>();
		public ICollection<EndpointDefinition> EndpointDefs { get; } = new List<EndpointDefinition>();


		public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
		{

			// mělo by to najít registrační
			//Debugger.Launch();

			if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax && classDeclarationSyntax.BaseList?.Types.Any(t => t.Type.ToString() == nameof(IApiEndpoint)) == true)
			{
				Endpoints.Add(classDeclarationSyntax);

				EndpointDefs.Add(new EndpointDefinition
				{
					Path = "",
					ClassName = classDeclarationSyntax.Identifier.ToFullString()
				});
			}
		}
	}
}
