using Microsoft.AspNetCore.Mvc;

namespace MinimalApiSourceGenerator.Endpoints
{
//[EndpointRegistration.IgnoreEndpoint]
	public class Detail3GetEndpoint
	{
		//public string Pattern => "/detail/{id}";
		public string Handler(int id, [FromRoute]int routeSimple)
		{
			return $"id: {id}";
		}
	}
}
