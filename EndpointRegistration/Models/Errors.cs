namespace EndpointRegistration.Models;
internal static class Errors
{
	public static readonly Diagnostic EntryPointNotFound = Diagnostic.Create(
		new DiagnosticDescriptor(
			"ER001", 
			nameof(EntryPointNotFound), 
			"Application entry point not found.", 
			nameof(EndpointsRegistrationGenerator), 
			DiagnosticSeverity.Error, 
			true),
		Location.None
	);

	public static Diagnostic InvalidEndpointDefinition(GeneratorException issue) => Diagnostic.Create(
		new DiagnosticDescriptor(
			"ER100", 
			nameof(InvalidEndpointDefinition), 
			$"Endpoint '{issue.ClassName ?? "<unknown_class>"}' won't be registered because of: '{issue}'", 
			nameof(EndpointFinder), 
			DiagnosticSeverity.Warning, 
			true),
		Location.None
	);
}
