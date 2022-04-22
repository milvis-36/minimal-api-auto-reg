namespace EndpointRegistration.Models;

internal record EndpointMappingInfo
{
	public EndpointMappingInfo(string interfaceName, string conventionName, string method)
	{
		InterfaceName = interfaceName;
		ConventionName = conventionName;
		Method = method;
	}

	public string InterfaceName { get; }

	public string ConventionName { get; }

	public string Method { get; set; }

	public void Deconstruct(out string interfaceName, out string conventionName, out string method)
	{
		interfaceName = InterfaceName;
		conventionName = ConventionName;
		method = Method;
	}
}
