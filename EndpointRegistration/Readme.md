# Minimal API Endpoint AutoRegistration 
**via Source code generators** to speedup development process. Without an expensive Reflection scan at the application startup.

## Usage

1. Add this nuget package
1. Rebuild solution to start the code generator.
1. Enable auto registration inside Program.cs

`var registrations = app.UseEndpointAutoRegister();`

This extension method returns an `Registrations` object that contains properties with all endpoints registrations as `RouteHandlerBuilder` to enable another configuration (see below).

`registrations.ProductDetailEndpoint.Produces(200)`

## Endpoints
Create a class that implements following rules.
You can skip an endpoint by using `IgnoreEndpoint` attribute.

### Interface Handler-based endpoint
Implement one of exposed interfaces 
- `IApiEndpoint` defaults to HTTP GET
- `IApiGetEndpoint`
- `IApiPutEndpoint`
- `IApiPostEndpoint`
- `IApiDeleteEndpoint`

All interfaces define a `Handler` method that handles HTTP request.

Additionally a class can implement `IRoutePattern` or `IApiRouteEndpoint`.
Both add a `Pattern` property used for a route template. 

### Conventional Name Handler-based endpoints
The source code generator looks for a classes named with a suffix
- `Endpoint` defaults to HTTP GET
- `GetEndpoint`
- `PostEndpoint`
- `PutEndpoint`
- `DeleteEndpoint`

The class has to declare `Handler` method.
The class could be even static with static method.

### Register-based endpoint
It's the most powerful options how to declare an api endpoint.
It could be either conventional or interface based:
- Implements `IAutoRegisterApiEndpoint`
- Name ends with one of these suffixes `AutoRegisterApiEndpoint`, `ArApiEndpoint`

The class has to declare `Register` method. It could be static as well.


## Endpoint structure
An endpoint is a class declaring:
1. Handler-based endpoints
   1. `Handler` method **(required)** - handling HTTP requests
   1. `Pattern` property *(optional)* - a route template declaration
   1. `Configure` method *(optional)* - an additional configuration of created `RouteHandlerBuilder`
1. Register-based endpoints
   1.  `RouteHandlerBuilder Register(IEndpointRouteBuilder app)` method **(required)** - should contain a complete endpoint declaration (Program.cs)

### Conventions
#### Endpoint name
A classname without endpoint suffix.
> DetailGetEndpoint => Detail

Applies to all endpoint types.

#### Route pattern
Ordered resolution:
1. `Pattern` property if not null
1. `RouteAttribute` applied on the `Handler` method
1. Endpoint name-based convetional
   1. Endpoint name (without the suffix) splited into words from the PascalCase
   1. Appended `Handler`'s arguments with applied `FromRouteAttribute` in same order
   1. If the first argument of `Handler` method is of primitive type (or string) then is used it as default

Applies to the Handler-based endpoints.

#### Additional route configuration
If you want to configure a created `RouteHandlerBuilder` then declare `Configure` method.

You can use it to define an Action name or a 'Produces' info.
> void Configure(RouteHandlerBuilder eb) => eb.WithName("Action1")

Applies to the Handler-based endpoints.

## Examples
### Endpoint
#### Static class conventional name (Handler-base)
```
public static class DetailGetEndpoint
{
	public static void Configure(RouteHandlerBuilder eb) => eb.WithName("ProductDetail");
	public static string Pattern => "/Detail/{id}";
	public static string Handler(int id)
	{
		return $"Should return product with id: {id}";
	}
}
```
#### Interface (Handler-based)
```
public class ListApiEndpoint : IApiEndpoint
{
	public void Configure(RouteHandlerBuilder eb)
	{
		eb
		 .Produces(200)
		 .Produces(404);
	}
	public string Pattern => "/list/index";
	public Delegate Handler { get; } = (HttpContext c) => { return $"Hello from {nameof(ListApiEndpoint)}: {c.Request.Path}"; };
}
```
#### Registration based
```
public static class DetailArApiEndpoint
{
	public static RouteHandlerBuilder Register(IEndpointRouteBuilder app)
	{
		return app
			.MapGet("/Detail/{id}", () => $"Should return product with id: {id}")
			.Produces(200);
	}
}
```

### Route patterns conventions
#### Endpoint name + FromRouteAttribute
```
public class DetailGetEndpoint
{
	public Delegate Handler => (int id, object o, MyClass c, [FromRoute] int r, [FromRoute(Name = "param-s")] int s) => $"id: {id}";
}
```
creates `app.MapGet("/detail1/{r}/{param-s}", <handler>)`

#### Endpoint name (multiple word) + first primitive argument
```
public class ProductsDetailGetEndpoint
{
	public Delegate Handler => (int id) => $"id: {id}";
}
```
creates `app.MapGet("/products/info/{id}", <handler>)`


### Supported declarations
#### Handler method
- `public Delegate Handler => (int id) => $"id: {id}";`
- `public string Handler(int id) => $"id: {id}";`
- `public static string Handler(int id) { return $"id: {id}"; }` 

#### Route Pattern property
- `public string Pattern => "/detail/{id}";`
- `public string Pattern { get; } = "/detail/{id}";`
- `public string Pattern { get { return "/detail/{id}"; } }`
- `public string Pattern { get => "/detail/{id}"; }`
- `public static string Pattern => "/Detail/{id}";` *same for static*

#### Configure method
- `public void Configure(RouteHandlerBuilder eb){ eb.Produces(200); }`
- `public string Configure(RouteHandlerBuilder eb) => eb.Produces(200);";`
- `public static string Configure => (RouteHandlerBuilder eb) => eb.Produces(200);`