namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<string> Category, string Description, string ImageFile, decimal Price);
    
    public record CreateProductResponse(Guid Id);

    public class CreateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // We get the request from the body of the HTTP POST request and map it to the CreateProductRequest object.
            // we process the request and return a response. (request is defined in this script)
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                // We use Mapster to map the request to the command object.
                // Command is defined in the CreateProductHandler.cs file. (request is redirected to the command handler)
                var command = request.Adapt<CreateProductCommand>();

                // We use MediatR to send the command to the command handler (CreateProductHandler.cs) that we mapped to.
                var result = await sender.Send(command);

                // We use Mapster to map the result to the response object.
                // Result is defined in the CreateProductHandler.cs file.
                var response = result.Adapt<CreateProductResponse>();

                // We return the response object with a 201 Created status code and the location of the created product.
                return Results.Created($"/products/{response.Id}", response);
            })
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create a new product")
            .WithDescription("Create a new product");

        }
    }
}
