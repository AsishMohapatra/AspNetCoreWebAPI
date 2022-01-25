# AspNetCoreWebAPI

 **ControllerBase** - Base Class for all the webAPIs.
 
 # ApiController Attribute
 -------------------------
 
 1. Can be applied on a class that is derived from ControllerBase.
 2. For a base controller we can have a class with no Route attrbute.
 3. [assembly: ApiController] applies web api behavior to all the controllers.

** Attribute routing Requirement **
	1. [ApiController] makes attribute routing a requirement.
	2. Actions are inaccessible by conventional route using #MapControllerRoute# function defined inside UseEndPoints.

** Binding source parameter inference **
	1. The complex object model binder pulls data from value providers in a defined order.
	2. SuppressInferBindingSourcesForParameters - API Behcior option to disable the inference.
		* [FromBody] * - data from Request body . 
				- Inferred for Complex Object type.
				- Is not inferred for simple types such as string or route.
				- If parameter are string/int but still need FromBody then explicit need of attribute.
				- When action has more than one parameter bound from request body. an exception is thrown.
		* [FromForm] * - data inferred when the type is IFormFile or IFormFileCollection.
		* [FromRoute] * - inferred for action parameter name matching to route template. Wont escape %2f.
		* [FromQuery] * - is inferred for anuother action parameter.
	
** Automatic HTTP 400 request **
	1. The [ApiController] attribute makes model validation errors automatically trigger an HTTP 400 response
	2. No need to check MOdelState.IsValid inside each action method.
	3. services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
		//Disable Automatic 400 response behavior.
        options.SuppressModelStateInvalidFilter = true;
	}

** Default Bad Request response **
	1. ValidationProblem method should be used instead of # BadRequest #
	2. ValidationProblem returns ValidationProblemDetails which get serialized as below.
{
"type":"",
"title":"",
"status":"400",
"traceId":"GUID",
"errors":
	{
	"":["Non empty request body is required"]
	}
}

** Problem details for error status code.
	1. Automatically converts the NotFound() or status codes greater than 400 to **ProblemDetails**
	2. Can be disabled by ** options.SuppressMapClientErrors = true; **

** Define Supported Request type using Consume attrbute **
	1. [Consumes] - Limits the supported media types.
	2. Request routed to action must have Content-Type header of application/xml that is the supported type.
	3. 415 or UnsupportedMedia type is thrown if Content-Type information is missing.
	4. The [Consumes] attribute also allows an action to influence its selection based on an incoming request's content type by applying a type constraint. 

[ApiController]
[Route("api/[controller]")]
public class ConsumesController : ControllerBase
{
    [HttpPost]
    [Consumes("application/json")]
    public IActionResult PostJson(IEnumerable<int> values) =>
        Ok(new { Consumes = "application/json", Values = values });

    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    public IActionResult PostForm([FromForm] IEnumerable<int> values) =>
        Ok(new { Consumes = "application/x-www-form-urlencoded", Values = values });
}


## Swagger Open API Specification ##

	1. Swagger is from SmartBear tooling for OpenAPI Specification.
	2. OpenAPI specification (openapi.json) or can be in YAML files
		- Document that describes the capabilities of your API. 
		- Document is based on the XML and attribute annotations within the controllers and models. 
		- It's the core part of the OpenAPI flow and is used to drive tooling such as SwaggerUI. By default, it's named openapi.json.
	3. Swashbuckle.AspNetCore
		-Swagger - Object model and middleware to expose the SwaggerDocument objects as JSON endpoints.
		-SwaggerGen - Exposes the swagger.json by by building it from routes , controllers and models.
		-SwaggerUI - Embedded version of swagger UI tool.
	4. Default location of swagger OAS document is  https://localhost:<port>/swagger/v1/swagger.json
	5. Default Url for Swagger UI is https://localhost:<port>/swagger
	6. Swagger endpoints for the json file should be a relative path using ./ 
			- services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("***SmartPrinter***", //*** API Name ***
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    { Title = "Printer Frame API", Version = "v1" });
            });
	7. app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/***SmartPrinter***/swagger.json", "Smart Printer API"); //Adds a swagger json endpoint
                    options.RoutePrefix = string.Empty;
                });
	7. How to add the XML comments to Swagger UI.
			- GenerateDocumentationFile =true in project file.
			- With this setting all the missing XML comments will be displayed as warning.
			- To Supress warning you can use as below using NoWarn
			-  <PropertyGroup>
					<GenerateDocumentationFile>true</GenerateDocumentationFile>
					<NoWarn>$(NoWarn);1591</NoWarn>
				</PropertyGroup>
			- To Suppress warning on specific class #pragma can be used as below.
					#pragma warning disable CS1591
					#pragma warning restore CS1591
			- Below configuration using SwaggerGenOptions can add XML document to swagger.
				// using System.Reflection;
						var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
						options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
						
			- summary: A high-level summary of what the method/class/field is or does.
			- remarks: More detail about the method/class/field.
			- param: A parameter to the method, and what it represents.
			- returns: A description of what the method returns.
			- ///<response code="204"> No Content</response>
			- [ProducesResponseType(StatusCodes.Status404NotFound)]
			
			- Identifies which content-types api handles using the attribute at controller level or action level.
					[Produces("application/json")]
					[Consumes("application/json")]