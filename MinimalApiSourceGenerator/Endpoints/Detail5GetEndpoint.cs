using Microsoft.AspNetCore.Mvc;

namespace MinimalApiSourceGenerator.Endpoints
{
	public class Detail5GetEndpoint
	{
		//public string Pattern => "/detail/{id}";
		[Route("/aaa/{r}")]
		public string Handler(int id, int r)
		{
			return $"id: {id}, r: {r}";
		}
	}
}
