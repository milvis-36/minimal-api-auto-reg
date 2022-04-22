using System.IO;
using EndpointRegistration.Models;
using Microsoft.CodeAnalysis.Text;

namespace EndpointRegistration;

[Generator]
public class EndpointsRegistrationGenerator : ISourceGenerator
{
#if DEBUG
	private const bool FlushToFile = true;
#endif

	public void Initialize(GeneratorInitializationContext context)
	{
		//if (!Debugger.IsAttached) Debugger.Launch();
		context.RegisterForSyntaxNotifications(() => new EndpointFinder());
	}

	public void Execute(GeneratorExecutionContext context)
	{
		//if (!Debugger.IsAttached) Debugger.Launch();
		var result = ((EndpointFinder)context.SyntaxReceiver)?.Result ?? new EndpointFinderResult();
		var endpointDefinitions = result.EndpointDefinitions;
		if (!endpointDefinitions.Any())
		{
			return;
		}

		var mainMethod = context.Compilation.GetEntryPoint(context.CancellationToken);
		if (mainMethod is null)
		{
			context.ReportDiagnostic(Errors.EntryPointNotFound);
			return;
		}

		var resolvedNamespace = mainMethod.ContainingNamespace.IsGlobalNamespace
			? Constants.DefaultNamespace
			: mainMethod.ContainingNamespace.ToDisplayString();
		var endpointsMapEndpoints = new StringBuilder();
		foreach (var (className, nameSpace, pattern, httpMethod) in endpointDefinitions)
		{
			endpointsMapEndpoints.AppendLine(
				@$"        {Constants.WebApplicationParamName}.Map{httpMethod}({pattern}, new {nameSpace}.{className}().{nameof(IApiEndpoint.Handler)});");
		}
		var registrationClassGenerated = BuildRegistrationClass(resolvedNamespace, endpointsMapEndpoints.ToString());
		
		context.AddSource($"{Constants.GeneratedFileName}.generated.cs", SourceText.From(registrationClassGenerated, Encoding.UTF8));

#if DEBUG
		if (FlushToFile)
		{
#pragma warning disable CS0162 // Unreachable code detected
			//File.WriteAllText(@$"./{Constants.GeneratedFileName}.flushed.cs", registrationClassGenerated, Encoding.UTF8);
			File.WriteAllText(@$"D:\temp\out\{Constants.GeneratedFileName}.flushed.cs", registrationClassGenerated, Encoding.UTF8);
#pragma warning restore CS0162 // Unreachable code detected
		}
#endif
	}

	private static string BuildRegistrationClass(string ns, string mapEndpoints) => $@"namespace {ns}
{{
  public static class {Constants.RegisterClassName}
  {{
      public static void {Constants.RegisterMethodName}(WebApplication {Constants.WebApplicationParamName})
      {{
{mapEndpoints}
      }}

      public static void UseEndpoint{Constants.RegisterMethodName}(this WebApplication {Constants.WebApplicationParamName})
      {{
        {Constants.RegisterMethodName}({Constants.WebApplicationParamName});
      }}
  }}
}}";
}
