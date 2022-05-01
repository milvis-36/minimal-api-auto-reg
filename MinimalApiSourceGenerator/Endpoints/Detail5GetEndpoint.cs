using Microsoft.AspNetCore.Mvc;

namespace MinimalApiSourceGenerator.Endpoints
{
	public class Detail5GetEndpoint
	{
		[Route("/aaa/{r}")]
		public string Handler(int id, int r)
		{
			return $"id: {id}, r: {r}";
		}
	}
}
