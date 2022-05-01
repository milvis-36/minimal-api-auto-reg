using EndpointRegistration.Strategies.Common;

namespace EndpointRegistration.Strategies.InterfaceBased;

internal class InterfaceEndpointFinder : HandlerDefinitionFinderBase
{
	public InterfaceEndpointFinder(IEndpointFinder? next = null) : base(next)
	{ }
	
	protected override EndpointMappingInfo? TryFindMapping(ClassDeclarationSyntax cls, List<EndpointMappingInfo> mappings)
		=> mappings.FirstOrDefault(m => cls.IsClassImplementingInterface(m.InterfaceName));

	protected override string GetEndpointSuffix(EndpointMappingInfo mapping)
		=> mapping.InterfaceName.Substring(1);
}
