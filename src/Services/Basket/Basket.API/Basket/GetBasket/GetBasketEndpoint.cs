namespace Basket.API.Basket.GetBasket
{
    // public record GetBasketRequest(string UserName); // this request input should match with the GetBasketResult to map correctly
    public record GetBasketResponse(ShoppingCart Cart);

    public class GetBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/{username}", async (string userName, ISender sender) =>
            {
                var result = await sender.Send(new GetBasketQuery(userName)); // MediatR

                var response = result.Adapt<GetBasketResponse>(); // Mapster

                return Results.Ok(response);
            })
                .WithName("GetBasket")
                .Produces<GetBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Get a basket by username")
                .WithDescription("Get a basket by username");
        }   
    }
}
