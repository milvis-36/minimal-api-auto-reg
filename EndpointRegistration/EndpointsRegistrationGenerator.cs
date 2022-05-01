using System.IO;
using Microsoft.CodeAnalysis.Text;

namespace EndpointRegistration;

[Generator]
public class EndpointsRegistrationGenerator : ISourceGenerator
{
#if DEBUG
	private const bool FlushToFile = false;
#endif

	public void Initialize(GeneratorInitializationContext context)
	{
		context.RegisterForSyntaxNotifications(() => new EndpointFinder());
	}

	public void Execute(GeneratorExecutionContext context)
	{
#if DEBUG  
		//if (!Debugger.IsAttached) Debugger.Launch();
#endif

		var (endpointDefinitions, exceptions) = (context.SyntaxReceiver as EndpointFinder)?.Result ?? new EndpointFinderResult();
		foreach (var generatorException in exceptions)
		{
			context.ReportDiagnostic(Errors.InvalidEndpointDefinition(generatorException));
		}

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

		var endpointInstances = new StringBuilder();
		var endpointMappings = new StringBuilder();
		var endpoints = new StringBuilder();

		foreach (var endpoint in endpointDefinitions)
		{
			ProcessEndpointDefinition(endpoint, endpointInstances, endpointMappings, endpoints);
		}

		var registrationClassGenerated = BuildRegistrationClass(resolvedNamespace, endpoints.ToString(), endpointInstances.ToString(), endpointMappings.ToString());

		context.AddSource($"{Constants.GeneratedFileName}.generated.cs", SourceText.From(registrationClassGenerated, Encoding.UTF8));

#if DEBUG
		if (FlushToFile)
		{
#pragma warning disable CS0162 // Unreachable code detected
			File.WriteAllText(@$"./{Constants.GeneratedFileName}.flushed.cs", registrationClassGenerated, Encoding.UTF8);
#pragma warning restore CS0162 // Unreachable code detected
		}
#endif
	}

	private static void ProcessEndpointDefinition(EndpointDefinition endpoint, StringBuilder endpointInstances, StringBuilder endpointMappings, StringBuilder endpoints)
	{
		var endpointName = endpoint.EndpointName;

		endpoints.AppendLine($"    public {Constants.RouteHandlerBuilder} {endpointName} {{ get; init; }}").AppendLine();

		endpointInstances.AppendLine($"        // {endpointName}");
		var variableName = endpointName.ToLowerInvariant();
		var endpointInstanceName = $"{variableName}Endpoint";

		EndpointGenerators
			.FirstOrDefault(endpointGenerator => endpointGenerator.CanProcess(endpoint))
			.Generate(endpoint, variableName, endpointInstanceName, endpointInstances);

		endpointMappings.AppendLine(@$"          {endpointName} = {endpointInstanceName},");
	}

	private static readonly
		List<(Func<EndpointDefinition, bool> CanProcess, Action<EndpointDefinition, string, string, StringBuilder> Generate)> EndpointGenerators = new()
			{
				(CanProcess: e => e.IsAutoRegister && e.IsStatic,
					Generate: (endpoint, variableName, endpointInstanceName, endpointInstances) =>
						endpointInstances.AppendLine(
							$"        var {endpointInstanceName} = {endpoint.Namespace}.{endpoint.Classname}.{Constants.AutoRegisterMethodName}({Constants.WebApplicationParamName});")
				),
				(CanProcess: e => e.IsAutoRegister && !e.IsStatic,
					Generate: (endpoint, variableName, endpointInstanceName, endpointInstances) =>
					{
						var (
							nameSpace,
							className
							) = endpoint;
						endpointInstances.AppendLine($"        var {variableName} = new {nameSpace}.{className}();");
						endpointInstances.AppendLine($"        var {endpointInstanceName} = {variableName}.{Constants.AutoRegisterMethodName}({Constants.WebApplicationParamName});");
					}
				),
				(CanProcess: e => !e.IsAutoRegister && e.IsStatic,
					Generate: (endpoint, variableName, endpointInstanceName, endpointInstances) =>
					{
						var (
							nameSpace,
							className,
							_,
							_,
							pattern,
							httpMethod,
							hasConfigureMethod,
							_
							) = endpoint;

						endpointInstances.AppendLine($"        var {endpointInstanceName} = {Constants.WebApplicationParamName}.Map{httpMethod}({pattern}, {nameSpace}.{className}.{nameof(IApiEndpoint.Handler)});");
						if (hasConfigureMethod)
						{
							endpointInstances.AppendLine($"        {nameSpace}.{className}.{Constants.ConfigureMethodName}({endpointInstanceName});");
						}
					}
				),
				(CanProcess: e => !e.IsAutoRegister && !e.IsStatic , Generate: (endpoint, variableName, endpointInstanceName,
					endpointInstances) =>
				{
					var (
						nameSpace,
						className,
						_,
						_,
						pattern,
						httpMethod,
						hasConfigureMethod,
						_
						) = endpoint;

					endpointInstances.AppendLine($"        var {variableName} = new {nameSpace}.{className}();");
					endpointInstances.AppendLine($"        var {endpointInstanceName} = {Constants.WebApplicationParamName}.Map{httpMethod}({pattern}, {variableName}.{nameof(IApiEndpoint.Handler)});");
					if (hasConfigureMethod)
					{
						endpointInstances.AppendLine($"        {variableName}.{Constants.ConfigureMethodName}({endpointInstanceName});");
					}
				}
				),
				(CanProcess: _ => true, Generate: (_, _, _, _) => { return; })
			};

	private static string BuildRegistrationClass(string ns, string endpoints, string endpointInstances, string endpointMappings) => $@"
namespace {ns}
{{
  public static class {Constants.RegisterClassName}
  {{
      public static {Constants.ResultClassName} {Constants.RegisterMethodName}({Constants.IEndpointRouteBuilder} {Constants.WebApplicationParamName})
      {{
{endpointInstances}
        return new {Constants.ResultClassName}
        {{
{endpointMappings}
        }};
      }}

      public static {Constants.ResultClassName} UseEndpoint{Constants.RegisterMethodName}(this {Constants.IEndpointRouteBuilder} {Constants.WebApplicationParamName})
        => {Constants.RegisterMethodName}({Constants.WebApplicationParamName});
  }}

  public record {Constants.ResultClassName}
  {{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
{endpoints}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
  }}
}}";
}
