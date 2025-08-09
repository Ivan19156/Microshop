using FluentValidation;
using Application.Products.Dtos;

namespace Application.Products.Validators;

    public class ProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва продукту обов'язкова.")
                .MaximumLength(100).WithMessage("Назва продукту не повинна перевищувати 100 символів.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Опис продукту обов'язковий.")
                .MaximumLength(500).WithMessage("Опис продукту не повинен перевищувати 500 символів.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Ціна повинна бути більшою за 0.");
        }
    }

