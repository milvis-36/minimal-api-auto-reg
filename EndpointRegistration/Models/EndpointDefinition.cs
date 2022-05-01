namespace EndpointRegistration.Models;

public record EndpointDefinition
{
	public string Namespace { get; set; } = nameof(Namespace);

	public string Classname { get; set; } = nameof(Classname);

	public bool IsStatic { get; set; } = false;

	public string EndpointName { get; set; } = nameof(EndpointName);

	public string Pattern { get; set; } = nameof(Pattern);

	public string HttpMethod { get; set; } = nameof(HttpMethod);

	public bool HasConfigureMethod { get; set; } = false;

	public bool IsAutoRegister { get; set; } = false;

	public void Deconstruct(out string ns, out string classname)
	{
		ns = Namespace;
		classname = Classname;
	}

	public void Deconstruct(out string ns, out string classname, out bool isStatic, out string endpointName)
	{
		ns = Namespace;
		classname = Classname;
		isStatic = IsStatic;
		endpointName = EndpointName;
	}

	public void Deconstruct(out string ns, out string classname, out bool isStatic, out string endpointName, out string pattern, out string httpMethod, out bool hasConfigureMethod, out bool isAutoRegister)
	{
		ns = Namespace;
		classname = Classname;
		isStatic = IsStatic;
		endpointName = EndpointName;
		pattern = Pattern;
		httpMethod = HttpMethod;
		hasConfigureMethod = HasConfigureMethod;
		isAutoRegister = IsAutoRegister;
	}
}
