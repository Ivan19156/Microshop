using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace FileService.Application.Validators;

public class UploadFileCommandValidator : AbstractValidator<IFormFile>
{
    public UploadFileCommandValidator()
    {
        RuleFor(file => file)
            .NotNull().WithMessage("Файл обов'язковий")
            .Must(file => file.ContentType == "image/jpeg" || file.ContentType == "image/png")
            .WithMessage("Підтримуються лише JPEG та PNG.");
    }
}
