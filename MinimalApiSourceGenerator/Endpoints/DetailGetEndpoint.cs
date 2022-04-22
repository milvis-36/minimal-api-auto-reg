namespace MinimalApiSourceGenerator.Endpoints
{
	public class DetailGetEndpoint
	{
		//public string Pattern => "/detail/{id}";
		public Delegate Handler => (int id) => $"id: {id}";
	}
}
