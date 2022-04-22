namespace MinimalApiSourceGenerator.Endpoints
{
	public class ProductInfoEndpoint
	{
		//public string Pattern => "/detail/{id}";
		public Delegate Handler => (int id) => $"id: {id}";
	}
}
