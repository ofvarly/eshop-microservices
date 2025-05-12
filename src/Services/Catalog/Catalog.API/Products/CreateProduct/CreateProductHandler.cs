
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


    // This is the request object that is used to map the data from the HTTP POST request to the command object.
    // It is used to validate the data before it is passed to the command handler. (Middleware)
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            // FluentValidation is a library for building strongly-typed validation rules.
            // It is used to validate the data in the command before it is passed to the command handler.
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }

    // IDocumentSession is a Marten interface that represents a session with the database.
    // It is used to perform CRUD operations on the database.
    // Why not use a repository pattern?
    // The repository pattern is an abstraction that provides a way to access data from a data source.
    // It is used to decouple the data access logic from the business logic.
    // The reason we don't use a repository pattern is that Marten provides a built-in way to access data from the database.
    internal class CreateProductCommandHandler(IDocumentSession session) : ICommandHandler<CreateProductCommand, CreateProductResult>
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

            // Mapper düzgün çalışıyor.
            // var product = command.Adapt<Product>();

            // TODO
            // save the product to the database
            session.Store(product);

            // cancellation token is used to cancel the operation if it takes too long
            await session.SaveChangesAsync(cancellationToken);


            return new CreateProductResult(product.Id); // Simulate product creation and return a new ID.
        }
    }

}
