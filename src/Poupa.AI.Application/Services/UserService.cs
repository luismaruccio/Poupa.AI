using FluentValidation;
using Microsoft.Extensions.Logging;
using Poupa.AI.Application.DTOs.Common;
using Poupa.AI.Application.DTOs.Users;
using Poupa.AI.Application.Interfaces.Services;
using Poupa.AI.Common.Extensions.Messages;
using Poupa.AI.Common.Utils;
using Poupa.AI.Domain.Interfaces.Repositories;
using Poupa.AI.Domain.Messages.Common;
using Poupa.AI.Domain.Messages.Users;

namespace Poupa.AI.Application.Services
{
    public class UserService(
        IUserRepository userRepository,
        IValidator<CreateUserRequest> createUserRequest,
        IHashService hashService,
        ILogger<UserService> logger) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IValidator<CreateUserRequest> _createUserRequest = createUserRequest;
        private readonly IHashService _hashService = hashService;
        private readonly ILogger<UserService> _logger = logger;
        
        public async Task<Either<FailureResponse, CreateUserResponse>> CreateUserAsync(CreateUserRequest request)
        {
            _logger.LogInformation("CreateUser - Request Received {request}", request);

            var validationResult = _createUserRequest.Validate(request);

            if (validationResult.Errors.Count > 0)
            {
                var error = validationResult.Errors.FirstOrDefault()!.ToString();
                _logger.LogError("CreateUser - Error on validate {error}", error);

                return Either<FailureResponse, CreateUserResponse>.FromError(
                    new FailureResponse(
                        ServicesMessages.AnErrorOccurred.WithParameters(
                            [
                                ServicesMessages.CreateOperation, 
                                UserMessages.User, 
                                error
                             ]
                        )
                    )
                );
            }

            var emailAlreadyInUse = await ValidateEmailAlreadyInUse(request.Email);

            if (emailAlreadyInUse)
            {
                _logger.LogError("CreateUser - Email already in use: {email}", request.Email);

                return Either<FailureResponse, CreateUserResponse>.FromError(
                    new FailureResponse(
                        ServicesMessages.AnErrorOccurred.WithParameters(
                            [
                                ServicesMessages.CreateOperation,
                                UserMessages.User,
                                UserMessages.EmailAlreadyInUse,
                            ]
                        )
                    )   
                );
            }

            var user = request.ToDomainEntity();
            user.Password = _hashService.HashPassword(request.Password);

            var createResult = await _userRepository.AddAsync(user);

            if (createResult.IsSuccess)
            {
                user = createResult.Success;
                _logger.LogInformation("CreateUser - User created sucessfully. Id {id}", user!.Id);
                return Either<FailureResponse, CreateUserResponse>.FromSuccess(CreateUserResponse.FromUser(user));
            }

            return Either<FailureResponse, CreateUserResponse>.FromError(
                new FailureResponse(
                    createResult.Error!
                )   
            );
        }

        private async Task<bool> ValidateEmailAlreadyInUse(string email)
        {
            var result = await _userRepository.GetByEmailAsync(email);

            return result.IsSuccess;
        }
    }
}
