using System.Diagnostics;

namespace MinimalApiSourceGenerator.Endpoints
{
	public class Detail2GetEndpoint
	{
		public Detail2GetEndpoint()
		{
			Debug.WriteLine(nameof(Detail2GetEndpoint));
			Console.WriteLine(nameof(Detail2GetEndpoint));
		}


		//public string Pattern => "/detail/{id}";

		public string Handler(int id)
		{
			return $"id: {id}";
		}
	}
}
