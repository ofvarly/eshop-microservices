
namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price); // this request input should match with the UpdateProductCommand to map correctly
    public record UpdateProductResponse(bool IsSuccess);

    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>(); // Mapster

                var result = await sender.Send(command); // MediatR
                
                var response = result.Adapt<UpdateProductResponse>(); // Mapster

                return Results.Ok(response); // return the response object
            })
                .WithName("UpdateProduct")
                .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Update a product")
                .WithDescription("Update a product");

        }
    }
}
