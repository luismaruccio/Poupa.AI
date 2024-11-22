using FluentValidation;
using Poupa.AI.Application.DTOs.Categories;
using Poupa.AI.Common.Extensions.Messages;
using Poupa.AI.Domain.Messages;
using Poupa.AI.Domain.Messages.Common;

namespace Poupa.AI.Application.Validators.Categories
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .NotEqual(0)
                .WithMessage(ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.Id));
            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotEqual(0)
                .WithMessage(ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.UserId));
            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.Name));
            RuleFor(c => c.Type)
                .NotEmpty()
                .WithMessage(ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.Type))
                .IsInEnum()
                .WithMessage(ValidatorsMessages.FieldInvalid.WithParameters(CategoryMessages.Type));
        }
    }
}
