using EndpointRegistration.Strategies.Common.RouteTemplate;

namespace EndpointRegistration.Strategies.Common;

internal abstract class HandlerDefinitionFinderBase : ClassDefinitionFinderBase
{
	private static readonly RouteTemplateResolver RoutePatternResolver = new();

	private EndpointMappingInfo? _mappingInfo;

	protected HandlerDefinitionFinderBase(IEndpointFinder? next = null) : base(next)
	{ }

	protected override bool IsEndpointDefinition(ClassDeclarationSyntax cls)
	{
		_mappingInfo = TryFindMapping(cls, Settings.Mappings);
		return _mappingInfo is not null;
	}

	protected override EndpointDefinition? ResolveClassDeclarationSyntax(
		ClassDeclarationSyntax cls)
	{
		var mapping = _mappingInfo;
		if (mapping is null) return null;

		try
		{
			var endpointName = ResolveEndpointName(cls, mapping);

			return new EndpointDefinition
			{
				HasConfigureMethod = ResolveHasConfigureMethod(cls),
				IsStatic = cls.Modifiers.IsStatic(),
				Pattern = GetRoutePattern(cls, endpointName),
				Classname = cls.GetIdentifier(),
				EndpointName = endpointName,
				Namespace = ResolveNamespace(cls),
				HttpMethod = ResolveHttpMethod(cls, mapping)
			};
		}
		catch (GeneratorException e)
		{
			e.ClassName = cls.GetIdentifier();
			throw;
		}
	}

	protected override void ValidateMandatoryStructure(ClassDeclarationSyntax cls)
	{
		const string handlerName = nameof(IApiEndpoint.Handler);
		var isHandlerMethodMissing =
			cls.GetPropertyByIdentifier(handlerName) is null && cls.GetMethodByIdentifier(handlerName) is null;
		if (isHandlerMethodMissing)
		{
			throw new GeneratorException($"{GetType().Name}: Endpoint is missing mandatory {handlerName} method.");
		}
	}

	protected abstract EndpointMappingInfo? TryFindMapping(ClassDeclarationSyntax cls, List<EndpointMappingInfo> mappings);

	protected abstract string GetEndpointSuffix(EndpointMappingInfo mapping);

	protected virtual string ResolveEndpointName(ClassDeclarationSyntax cls, EndpointMappingInfo mapping)
	{
		var identifier = cls.GetIdentifier();
		var endpointSuffix = GetEndpointSuffix(mapping);
		var endIndex = identifier.LastIndexOf(endpointSuffix, StringComparison.InvariantCultureIgnoreCase);
		if (endIndex == -1)
		{
			throw new GeneratorException($"{GetType().Name}: Cannot resolve endpoint name. Classname: '{identifier}', Suffix: '{endpointSuffix}'");
		}

		return identifier.Substring(0, Math.Max(endIndex, 0));
	}

	

	protected virtual string GetRoutePattern(ClassDeclarationSyntax cls, string endpointName)
		=> RoutePatternResolver.GetRoutePattern(cls, endpointName);

	protected virtual string ResolveHttpMethod(ClassDeclarationSyntax cls, EndpointMappingInfo mapping)
		=> mapping.Method;

	protected virtual bool ResolveHasConfigureMethod(ClassDeclarationSyntax cls)
		=> cls.GetPropertyByIdentifier(Constants.ConfigureMethodName) is not null ||
		   cls.GetMethodByIdentifier(Constants.ConfigureMethodName) is not null;
}
