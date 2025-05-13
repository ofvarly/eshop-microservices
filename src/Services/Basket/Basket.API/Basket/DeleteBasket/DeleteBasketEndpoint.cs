namespace Basket.API.Basket.DeleteBasket
{
    //public record DeleteBasketRequest(string UserName);

    public record DeleteBasketResponse(bool IsSuccess); // this response output should match with the DeleteBasketResult to map correctly

    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{username}", async (string userName, ISender sender) =>
            { 
                var result = await sender.Send(new DeleteBasketCommand(userName)); // MediatR

                var response = result.Adapt<DeleteBasketResponse>(); // Mapster

                return Results.Ok(response); // return the response object
            })
                .WithName("DeleteBasket")
                .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithSummary("Delete a basket")
                .WithDescription("Delete a basket");
        }
    }
}
