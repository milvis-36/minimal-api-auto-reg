using Microsoft.AspNetCore.Mvc;

namespace MinimalApiSourceGenerator.Endpoints
{
	public class Detail4GetEndpoint
	{
		public string Handler(int id, [FromRoute(Name = "route")]int r)
		{
			return $"id: {id}";
		}
	}
}
