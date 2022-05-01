using EndpointRegistration.Strategies.Common;

namespace EndpointRegistration.Strategies.Conventional;

internal class ConventionalEndpointFinder : HandlerDefinitionFinderBase
{
	public ConventionalEndpointFinder(IEndpointFinder? next = null) : base(next)
	{ }

	protected override EndpointMappingInfo? TryFindMapping(ClassDeclarationSyntax cls, List<EndpointMappingInfo> mappings)
	{
		var className = cls.GetIdentifier();

		return mappings.FirstOrDefault(x => className.EndsWith(x.ConventionName));
	}

	protected override string GetEndpointSuffix(EndpointMappingInfo mapping)
		=> mapping.ConventionName;
	}
