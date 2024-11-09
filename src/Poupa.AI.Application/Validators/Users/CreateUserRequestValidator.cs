using FluentValidation;
using Poupa.AI.Application.DTOs.Users;
using Poupa.AI.Common.Extensions.Messages;
using Poupa.AI.Domain.Messages.Common;
using Poupa.AI.Domain.Messages.Users;

namespace Poupa.AI.Application.Validators.Users
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator() 
        {
            RuleFor(u => u.Name)
                .NotEmpty()
                .WithMessage(ValidatorsMessages.FieldRequired.WithParameters(UserMessages.Name));
            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage(ValidatorsMessages.FieldRequired.WithParameters(UserMessages.Email))
                .EmailAddress()
                .WithMessage(ValidatorsMessages.FieldInvalid.WithParameters(UserMessages.Email));
            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage(ValidatorsMessages.FieldRequired.WithParameters(UserMessages.Password));
        
        
        }
    }
}
