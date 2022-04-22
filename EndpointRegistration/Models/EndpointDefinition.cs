namespace EndpointRegistration.Models;

public record EndpointDefinition
{
	public string ClassName { get; set; }
	public string Namespace { get; set; }

	public string Pattern { get; set; }

	public string HttpMethod { get; set; }

	public void Deconstruct(out string className, out string nameSpace, out string pattern, out string httpMethod)
	{
		className = ClassName;
		nameSpace = Namespace;
		pattern = Pattern;
		httpMethod = HttpMethod;
	}
}
