
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);
    public record StoreBasketResponse(string UserName);

    public class StoreBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>(); // Mapster

                var result = await sender.Send(command); // MediatR
                
                var response = result.Adapt<StoreBasketResponse>(); // Mapster

                return Results.Created($"/basket/{response.UserName}", response);
            })
                .WithName("StoreBasket")
                .Produces<StoreBasketResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Store a shopping cart")
                .WithDescription("Store a shopping cart");
        }
    }
}
