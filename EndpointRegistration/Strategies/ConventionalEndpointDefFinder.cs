using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EndpointRegistration.Strategies;
internal class ConventionalEndpointDefFinder : ClassDefinitionSyntaxFinderBase
{
	public ConventionalEndpointDefFinder(IEndpointFinder? next = null) : base(next)
	{ }

	protected override EndpointDefinition? ResolveClassDeclarationSyntax(ClassDeclarationSyntax cls)
	{
		EndpointMappingInfo? mapping = TryFindMapping(cls);
		if (mapping is null)
		{
			return null;
		}

		var identifier = cls.GetIdentifier();

		return new EndpointDefinition
		{
			Pattern = PatternProperty ?? ResolvePattern(identifier, mapping.ConventionName),
			ClassName = cls.GetIdentifier(),
			Namespace = Namespace,
			HttpMethod = mapping.Method
		};
	}

	private static string ResolvePattern(string identifier, string suffix)
	{
		//todo projít podle velkých písmen a rozdělit
		// a ještě brát v potaz parametry 
		// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0

		return $"\"{ConvertToPattern(identifier, suffix)}\"";
	}

	private static string ConvertToPattern(string s, string suffix)
	{
		var end = s.Length - suffix.Length; // pak to bude end - endpoint.leght
		var sb = new StringBuilder(end + 5);
		for (int i = 0; i < end; i++)
		{
			var c = s[i];
			if (char.IsUpper(c))
			{
				sb.Append("/");
			}
			sb.Append(char.ToLowerInvariant(c));
		}

		return sb.ToString();
	}

	private static EndpointMappingInfo? TryFindMapping(ClassDeclarationSyntax cls)
	{
		var className = cls.GetIdentifier();
		return Settings.Mappings.FirstOrDefault(x => className.EndsWith(x.ConventionName));

	}
}
