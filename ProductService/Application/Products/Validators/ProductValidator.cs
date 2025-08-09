using FluentValidation;
using Application.Products.Dtos;

namespace Application.Products.Validators;

    public class ProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("����� �������� ����'������.")
                .MaximumLength(100).WithMessage("����� �������� �� ������� ������������ 100 �������.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("���� �������� ����'�������.")
                .MaximumLength(500).WithMessage("���� �������� �� ������� ������������ 500 �������.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("ֳ�� ������� ���� ������ �� 0.");
        }
    }

