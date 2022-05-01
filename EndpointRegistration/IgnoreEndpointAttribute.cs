namespace EndpointRegistration;

[AttributeUsage(AttributeTargets.Class)]
public class IgnoreEndpointAttribute : Attribute
{
	public static readonly string AttributeName = nameof(IgnoreEndpointAttribute).Substring(0,
		nameof(IgnoreEndpointAttribute).IndexOf(nameof(Attribute), StringComparison.Ordinal));
}
