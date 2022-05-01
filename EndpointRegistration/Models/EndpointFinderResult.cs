namespace EndpointRegistration.Models;

internal record EndpointFinderResult
{
	public ICollection<EndpointDefinition> EndpointDefinitions { get; } = new List<EndpointDefinition>();
	
	public ICollection<GeneratorException> GeneratorExceptions{ get; } = new List<GeneratorException>();

	public void Deconstruct(out ICollection<EndpointDefinition> endpointDefinitions, out ICollection<GeneratorException> generatorExceptions)
	{
		endpointDefinitions = EndpointDefinitions;
		generatorExceptions = GeneratorExceptions;
	}
}
