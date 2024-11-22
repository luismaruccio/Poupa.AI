using FluentValidation;
using Poupa.AI.Application.DTOs.Categories;
using Poupa.AI.Common.Extensions.Messages;
using Poupa.AI.Domain.Messages;
using Poupa.AI.Domain.Messages.Common;

namespace Poupa.AI.Application.Validators.Categories
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.Name));
            RuleFor(c => c.Type)
                .NotEqual(Domain.Enums.TransactionType.None)
                .WithMessage(ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.Type))
                .IsInEnum()
                .WithMessage(ValidatorsMessages.FieldInvalid.WithParameters(CategoryMessages.Type));
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotEqual(0)
                .WithMessage(ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.UserId));
        }
    }
}
