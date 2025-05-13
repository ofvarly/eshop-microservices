namespace Catalog.API.Products.DeleteProduct
{
    // public record DeleteProductRequest(Guid Id); // this request input should match with the DeleteProductCommand to map correctly

    public record DeleteProductResponse(bool IsSuccess);

    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(id)); // MediatR

                var response = result.Adapt<DeleteProductResponse>(); // Mapster

                return Results.Ok(response); // return the response object
            })
                .WithName("DeleteProduct")
                .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete a product")
                .WithDescription("Delete a product");
        }
    }
}
