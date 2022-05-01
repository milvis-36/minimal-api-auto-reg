namespace EndpointRegistration.Strategies.Common.RouteTemplate.Strategies.RouteParams.Strategies;

internal class ConventionalStrategy : IRouteParamsFinder
{
	private static readonly ICollection<string> PrimitiveTypes = new HashSet<string>
	{
		"byte",
		"sbyte",
		"short",
		"ushort",
		"int",
		"uint",
		"long",
		"ulong",
		"float",
		"double",
		"decimal",
		"char",
		"bool",
		"string",
	};

	public string[]? GetRouteParams(ParameterListSyntax parameterListSyntax)
	{
		var parameter = parameterListSyntax.Parameters.FirstOrDefault();
		var type = TryGetTypeKeyword(parameter);
		if (parameter is not null 
		    && type is not null 
		    && PrimitiveTypes.Contains(type))
		{
			return new []{ parameter.Identifier.ValueText };
		}

		return  null;
	}

	private static string? TryGetTypeKeyword(ParameterSyntax? p)
		=> (p?.Type as PredefinedTypeSyntax)?.Keyword.ValueText;

}
