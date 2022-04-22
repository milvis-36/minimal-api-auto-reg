namespace EndpointRegistration.Models;

internal record EndpointFinderResult
{
	public ICollection<EndpointDefinition> EndpointDefinitions { get; } = new List<EndpointDefinition>();
	public void Deconstruct(out ICollection<EndpointDefinition> endpointDefinitions)
	{
		endpointDefinitions = EndpointDefinitions;
	}
}
