using BuildingBlocks.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct
{
    // Why record?
    // A record is a reference type that provides built-in functionality for encapsulating data.

    // This is the command to create a product, it allows us to encapsulate all the data needed to create a product in a single object.
    // This is useful to pass the data to the command handler and to validate the data before creating the product.
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;

    // This is the result of the command, it contains the ID of the created product.
    // This is useful to return the ID of the product to the client after it has been created.
    public record CreateProductResult(Guid Id);

    internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create a new product with the data from the command
            // save to database
            // return CreateProductResult result

            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            // TODO
            // save the product to the database

            return new CreateProductResult(Guid.NewGuid()); // Simulate product creation and return a new ID.
        }
    }

}
