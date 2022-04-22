using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGenerators.Infrastructure
{


	[Generator]
	public class EndpointsRegistrationGenerator : ISourceGenerator
	{
		public void Initialize(GeneratorInitializationContext context)
		{
			// TODO: rozšířit pro nalezení registration
			context.RegisterForSyntaxNotifications(() => new EndpointFinder());
		}

		public void Execute(GeneratorExecutionContext context)
		{
			return;
			// bude se to dít, jen když budou endpointy
			// kontrola registrační classy + partial
			context.ReportDiagnostic(
				Diagnostic.Create(
					new DiagnosticDescriptor("REG001", "Chyba", "aaaa", "yay", DiagnosticSeverity.Error, true),
					Location.None
					
					));

			var endpoints =
				((EndpointFinder)context.SyntaxReceiver)?.Endpoints ?? Array.Empty<ClassDeclarationSyntax>();

			var defs =
				((EndpointFinder)context.SyntaxReceiver)?.EndpointDefs ?? Array.Empty<EndpointDefinition>();
			//Debugger.Launch();

			var sb = new StringBuilder(@"
namespace MinimalApiSourceGeneratorGenerated
{
  public static class RegistrationXXX
  {
    public static void Reg(WebApplication app)
    {
");
			// vzít tu definici najít metody podle Interface
			//foreach (var classDeclarationSyntax in endpoints)
			//{

			//}

			sb.AppendLine(@"
    }
  }
}
");

			context.AddSource("AutoRegistration", SourceText.From(sb.ToString(), Encoding.UTF8));
			context.AddSource("TTT", SourceText.From(@"namespace SSS { public static class Ccc { public static void Test() { } } }", Encoding.UTF8));


			//var controllers =
			//	context.Compilation
			//		.SyntaxTrees
			//		.SelectMany(syntaxTree => syntaxTree.GetRoot().DescendantNodes())
			//		.Where(x => x is ClassDeclarationSyntax)
			//		.Cast<ClassDeclarationSyntax>()
			//		.Where(c => c.Identifier.ValueText.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
			//		.ToImmutableList();



			//			context.AddSource("GeneratedReg", SourceText.From(@"using System;
			//namespace HelloWorldGenerated
			//{}"));

			//context.Compilation.SyntaxTrees.SelectMany(t => t.)
		}
	}
}
