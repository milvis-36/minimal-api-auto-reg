namespace EndpointRegistration.Models;
internal static class Errors
{
	public static readonly Diagnostic EntryPointNotFound = Diagnostic.Create(
		new DiagnosticDescriptor("ER001", "Entry point not found", "Application entry point not found.", nameof(EndpointsRegistrationGenerator), DiagnosticSeverity.Error, true),
		Location.None
	);
}
