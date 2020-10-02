namespace SampleProject.Application.Orders.PlaceCustomerOrder
{
    using FluentValidation;

    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0)
                .WithMessage("At least one product has invalid quantity");
        }
    }
}