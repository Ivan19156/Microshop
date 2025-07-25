using Application.Products.Dtos;
using FluentValidation;
using Application.Products.Dtos;

namespace Application.Products.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
        RuleFor(p => p.Description).MaximumLength(500);
        RuleFor(p => p.Price).GreaterThanOrEqualTo(0);
        RuleFor(p => p.Quantity).GreaterThanOrEqualTo(0);
    }
}
