using System.Runtime.Serialization;

namespace EndpointRegistration.Models;

public class GeneratorException : Exception
{
	public string? ClassName { get; set; }

	public GeneratorException()
	{
	}

	protected GeneratorException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	public GeneratorException(string message) : base(message)
	{
	}

	public GeneratorException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
