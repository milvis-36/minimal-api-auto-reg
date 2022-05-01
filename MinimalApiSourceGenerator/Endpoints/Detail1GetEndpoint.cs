using Microsoft.AspNetCore.Mvc;

namespace MinimalApiSourceGenerator.Endpoints;

public class Detail1GetEndpoint
{
	public string Pattern => "/detail/{id}";

	

	public Delegate Handler0 => (int id) => $"id: {id}";
	public Delegate Handler => (int id, object o, MyClass c, [FromRoute] int r, [FromRoute(Name = "param-s")] int s) => $"id: {id}";

	[Route("aaaaaaa/aaa")]
	public string Handler2(int id)
	{
		return $"id: {id}";
	}
}

class MyClass
{
	
}