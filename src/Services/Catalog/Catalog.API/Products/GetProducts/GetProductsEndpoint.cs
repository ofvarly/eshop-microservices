
namespace Catalog.API.Products.GetProducts
{
    public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10); // this request input should match with the GetProductsQuery to map correctly
    public record GetProductsResponse(IEnumerable<Product> Products); // this response output should match with the GetProductsResult to map correctly

    public class GetProductsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // [AsParameters] is used to bind the query parameters to the request object
            app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetProductsQuery>(); // Mapster

                var result = await sender.Send(query); // MediatR

                var response = result.Adapt<GetProductsResponse>(); // Mapster

                return Results.Ok(response);
            })
                .WithName("GetProducts")
                .Produces<GetProductsResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get all products")
                .WithDescription("Get products");
        }
    }
}