namespace Generated
{
  public static class EndpointsX
  {
    public static void AutoRegister(WebApplication app)
    {
      app.MapGet("/detail/{id}", new MinimalApiSourceGenerator.Endpoints.DetailGetEndpoint().Handler);
      app.MapGet("/home/index", new MinimalApiSourceGenerator.Endpoints.Products.ListApiEndpoint().Handler);

    }

    public static void UseEndpointAutoRegister(this WebApplication app)
    {
      AutoRegister(app);
	  }
  }
}