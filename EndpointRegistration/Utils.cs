using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EndpointRegistration;

internal class Utils
{
	private static class HttpMethods
	{
		public const string Get = "Get";
		public const string Post = "Post";
		public const string Put = "Put";
		public const string Delete = "Delete";
	}

	private static readonly List<(string interfaceName, string conventionName, string method)> Config = new()
	{
		(nameof(IApiGetEndpoint), $"{HttpMethods.Get}{Constants.EndpointSuffix}", HttpMethods.Get),
		(nameof(IApiPostEndpoint), $"{HttpMethods.Post}{Constants.EndpointSuffix}", HttpMethods.Post),
		(nameof(IApiPutEndpoint), $"{HttpMethods.Put}{Constants.EndpointSuffix}", HttpMethods.Put),
		(nameof(IApiDeleteEndpoint), $"{HttpMethods.Delete}{Constants.EndpointSuffix}", HttpMethods.Delete),
		(nameof(IApiEndpoint), Constants.EndpointSuffix, HttpMethods.Get),
	};

	public static bool IsConventionRegistration(SyntaxNode syntaxNode)
	{
		if (syntaxNode is not ClassDeclarationSyntax cls)
		{
			return false;
		}

		var className = cls.Identifier.ToString();

		return Config.Any(x => className.EndsWith(x.conventionName));
	}

	public static string ResolveHttpMethod(ClassDeclarationSyntax cls)
		=> TryResolveFromClassName(cls) ?? TryResolveFromInterface(cls);

	public static string ResolvePattern(ClassDeclarationSyntax cls)
	{
		var identifier = cls.GetIdentifier();
		if (identifier is null)
		{
			return null;
		}

		var conventionalName = Config.FirstOrDefault(x => identifier.Contains(x.conventionName)).conventionName;
		if (conventionalName is null)
		{
			return null;
		}

		var name = identifier.Replace(conventionalName, string.Empty);
		//todo projít podle velkých písmen a rozdělit
		// a ještě brát v potaz parametry 
		// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0

		return $"\"{ConvertToPattern(identifier, conventionalName)}\"";

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

	private static string TryResolveFromInterface(SyntaxNode cls)
	{
		foreach (var (interfaceName, _, method) in Config)
		{
			if (cls.IsClassImplementingInterface(interfaceName))
			{
				return method;
			}
		}

		return null;
	}

	public static string TryResolveFromClassName(ClassDeclarationSyntax cls)
	{
		var identifier = cls.GetIdentifier();
		//TODO: nullable
		return Config.FirstOrDefault(x => x.conventionName is not null && identifier.Contains(x.conventionName)).method;
	}
}