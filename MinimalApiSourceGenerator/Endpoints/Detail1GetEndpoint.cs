using Microsoft.AspNetCore.Mvc;

namespace MinimalApiSourceGenerator.Endpoints;

//[EndpointRegistration.IgnoreEndpoint]
public class Detail1GetEndpoint
{
	public string Pattern0 => "/detail/{id}";

	

	public Delegate Handler0 => (int id) => $"id: {id}";
	public Delegate Handler => ([FromQuery]int id, [FromBody] MyClass c, [FromRoute] int r, [FromRoute(Name = "param-s")] int s) => $"id: {id}";

	[Route("aaaaaaa/aaa")]
	public string Handler2(int id)
	{
		return $"id: {id}";
	}
}

class MyClass
{
	public string Sss { get; set; }
}