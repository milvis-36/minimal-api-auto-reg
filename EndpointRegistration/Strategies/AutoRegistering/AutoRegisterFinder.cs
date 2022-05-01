using EndpointRegistration.Strategies.Common;

namespace EndpointRegistration.Strategies.AutoRegistering;

internal class AutoRegisterFinder : ClassDefinitionFinderBase
{
	public AutoRegisterFinder(IEndpointFinder? next = null) : base(next)
	{ }

	protected override bool IsEndpointDefinition(ClassDeclarationSyntax cls)
		=> Constants.AutoRegisterEndpointSuffixes.Any(cls.IdentifierEndsWith) ||
		   cls.IsClassImplementingInterface(nameof(IAutoRegisterApiEndpoint));

	protected override void ValidateMandatoryStructure(ClassDeclarationSyntax cls)
	{
		//var m1 = cls.GetPropertyByIdentifier(Constants.AutoRegisterMethodName);
		var registerReturnTypeName = (cls.GetMethodByIdentifier(Constants.AutoRegisterMethodName)?.ReturnType as IdentifierNameSyntax)?.Identifier.ToString();

		if (!Constants.AutoRegisterMethodReturnType.Equals(registerReturnTypeName))
		{
			throw new GeneratorException($"{nameof(AutoRegisterFinder)}: Auto register endpoint has to declare '{Constants.AutoRegisterMethodReturnType} {Constants.AutoRegisterMethodName}({Constants.IEndpointRouteBuilder} {Constants.WebApplicationParamName}) ' method.")
			{
				ClassName = cls.GetIdentifier()
			};
		}
	}

	protected override EndpointDefinition? ResolveClassDeclarationSyntax(ClassDeclarationSyntax cls)
	{
		var className = cls.GetIdentifier();
		var suffix = Constants.AutoRegisterEndpointSuffixes.FirstOrDefault(suffix => className.EndsWith(suffix)) ?? Constants.EndpointSuffix;
		var endpointName = className.Substring(0, className.Length - suffix.Length);

		return new EndpointDefinition
		{
			Namespace = ResolveNamespace(cls),
			IsStatic = cls.Modifiers.IsStatic(),
			Classname = cls.GetIdentifier(),
			EndpointName = endpointName,
			IsAutoRegister = true
		};
	}
}

