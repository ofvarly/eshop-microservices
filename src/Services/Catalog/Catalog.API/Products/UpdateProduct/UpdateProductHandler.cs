
using Catalog.API.Products.CreateProduct;
using Mapster;

namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
        : ICommand<UpdateProductResult>;

    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Product ID is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
                .Length(2,150).WithMessage("Name must be between 2 and 150 characters.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }



    internal class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        //public static void ConfigureMapping()
        //{
        //    // Product → UpdateProductCommand mapleme konfigürasyonu
        //    TypeAdapterConfig<Product, UpdateProductCommand>
        //        .NewConfig()
        //        .Ignore(dest => dest.Id); // Id özelliğini hariç tut
        //}

        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductCommandHandler.Handle called with {@Command}", command);

            var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        
            if (product is null)
                throw new ProductNotFoundException(command.Id);

            // Configure Mapping'e gerek olmadan doğru bir şekilde mapleme yapılıyor.
            // product = command.Adapt<Product>();

            product.Name = command.Name;
            product.Category = command.Category;
            product.Description = command.Description;
            product.ImageFile = command.ImageFile;
            product.Price = command.Price;

            session.Update(product);

            await session.SaveChangesAsync(cancellationToken);

            return new UpdateProductResult(true);
        }
    }

}
