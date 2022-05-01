using System.Diagnostics;
using EndpointRegistration;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApiSourceGenerator.Endpoints;

//[IgnoreEndpoint]
public static class Detail0GetEndpoint
{
	public static void Configure(RouteHandlerBuilder eb) => eb.WithName("aaa").Produces(500);

	public static string Pattern0 => "/Detail0/{id}";


	//public static string Handler(int id)
	//{
	//	return $"id: {id}";
	//}

	public static string Handler(int id)
		=> $"id: {id}";
	
}
