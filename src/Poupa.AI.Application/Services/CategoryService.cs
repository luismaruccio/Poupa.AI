using FluentValidation;
using Microsoft.Extensions.Logging;
using Poupa.AI.Application.DTOs.Categories;
using Poupa.AI.Application.DTOs.Common;
using Poupa.AI.Application.Interfaces.Services;
using Poupa.AI.Common.Extensions.Messages;
using Poupa.AI.Common.Utils;
using Poupa.AI.Domain.Enums;
using Poupa.AI.Domain.Interfaces.Repositories;
using Poupa.AI.Domain.Messages; 
using Poupa.AI.Domain.Messages.Common;

namespace Poupa.AI.Application.Services
{
    public class CategoryService(
        ICategoryRepository categoryRepository,
        IValidator<CreateCategoryRequest> createCategoryRequestValidator,
        IValidator<UpdateCategoryRequest> updateCategoryRequestValidator,
        ILogger<CategoryService> logger) : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IValidator<CreateCategoryRequest> _createCategoryRequestValidator = createCategoryRequestValidator;
        private readonly IValidator<UpdateCategoryRequest> _updateCategoryRequestValidator = updateCategoryRequestValidator;
        private readonly ILogger<CategoryService> _logger = logger;

        public async Task<Either<MessageResponse, CategoryResponse>> CreateCategoryAsync(CreateCategoryRequest request)
        {
            _logger.LogInformation("CreateCategoryAsync - Request Received {request}", request);
            
            var validationResult = _createCategoryRequestValidator.Validate(request);

            if (validationResult.Errors.Count > 0)
            {
                var error = validationResult.Errors.FirstOrDefault()!.ToString();
                _logger.LogError("CreateCategoryAsync - Error on validate {error}", error);

                return Either<MessageResponse, CategoryResponse>.FromError(
                    new MessageResponse(
                        ServicesMessages.AnErrorOccurred.WithParameters(
                            [
                                ServicesMessages.CreateOperation,
                                CategoryMessages.Category,
                                error
                            ]
                        )
                    )
                );
            }

            var category = request.ToDomainEntity();
            var result = await _categoryRepository.AddAsync( category );

            if (result.IsSuccess)
            {
                category = result.Success;
                _logger.LogInformation("CreateCategoryAsync - Category created sucessfully. Category: {category}", category);
                return Either<MessageResponse, CategoryResponse>.FromSuccess(CategoryResponse.FromDomainEntity(category!));
            }

            _logger.LogInformation("CreateCategoryAsync - Error on create category. Category: {category}. Error {error}", [category, result.Error!]);
            return Either<MessageResponse, CategoryResponse>.FromError(
                new MessageResponse(
                    result.Error!
                )
            );
        }

        public async Task<Either<MessageResponse, MessageResponse>> DeleteCategoryAsync(int id)
        {
            _logger.LogInformation("DeleteCategoryAsync - Request for soft delete received for {id}", id);

            if (id <= 0) {

                var error =  ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.Id);
                _logger.LogError("DeleteCategoryAsync - Error on validate {error}", error);

                return Either<MessageResponse, MessageResponse>.FromError(
                    new MessageResponse(
                        ServicesMessages.AnErrorOccurred.WithParameters(
                            [
                                ServicesMessages.DeleteOperation,
                                CategoryMessages.Category,
                                error
                            ]
                        )
                    )
                );
            }

            var result = await _categoryRepository.DeleteAsync( id );

            if (result.IsSuccess)
            {
                _logger.LogInformation("DeleteCategoryAsync - Category removed sucessfully. Id {id}", id);
                return Either<MessageResponse, MessageResponse>.FromSuccess(
                    new MessageResponse(
                        ServicesMessages.Success.WithParameters([CategoryMessages.Category, ServicesMessages.Removed])
                    )
                );
            }

            _logger.LogInformation("CreateCategoryAsync - Error on removing category. CategoryId: {id}. Error {error}", [id, result.Error!]);
            return Either<MessageResponse, MessageResponse>.FromError(
                new MessageResponse(
                    result.Error!
                )
            );

        }

        public async Task<Either<MessageResponse, IReadOnlyList<CategoryResponse>>> GetCategoriesByUserAndTypeAsync(int userId, TransactionType type)
        {
            _logger.LogInformation("GetCategoriesByUserAndTypeAsync - Request for get categories by userId and type ({id},{type})", [userId, type]);

            var errors = new List<string>();

            if (userId <= 0)
            {
                var error = ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.UserId);
                _logger.LogError("GetCategoriesByUserAndTypeAsync - Error on validate {error}", error);
                errors.Add( error );
            }

            if (type == TransactionType.None)
            {
                var error = ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.Type);
                _logger.LogError("GetCategoriesByUserAndTypeAsync - Error on validate {error}", error);
                errors.Add(error);
            }

            if (errors.Count > 0)
            {
                return Either<MessageResponse, IReadOnlyList<CategoryResponse>>.FromError(
                    new MessageResponse(
                        ServicesMessages.AnErrorOccurred.WithParameters(
                            [
                                ServicesMessages.DeleteOperation,
                                CategoryMessages.Category,
                                string.Join(Environment.NewLine, errors)
                            ]
                        )
                    )
                );
            }

            

            var result = await _categoryRepository.GetAllByUserAndTypeAsync(userId, type);

            if (result.IsSuccess)
            {
                _logger.LogInformation("GetCategoriesByUserAndTypeAsync - Categories got sucessfully");
                return Either<MessageResponse, IReadOnlyList<CategoryResponse>>.FromSuccess(CategoryResponse.FromDomainEntities(result.Success!));
            }

            _logger.LogInformation("GetCategoriesByUserAndTypeAsync - Error on getting categories.");
            return Either<MessageResponse, IReadOnlyList<CategoryResponse>>.FromError(
                new MessageResponse(
                    result.Error!
                )
            );
        }

        public async Task<Either<MessageResponse, CategoryBreakdownResponse>> GetCategoriesByUserAsync(int userId)
        {
            _logger.LogInformation("GetCategoriesByUserAsync - Request for get categories by userId {id}", userId);

            if (userId <= 0)
            {
                var error = ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.UserId);
                _logger.LogError("GetCategoriesByUserAsync - Error on validate {error}", error);

                return Either<MessageResponse, CategoryBreakdownResponse>.FromError(
                    new MessageResponse(
                        ServicesMessages.AnErrorOccurred.WithParameters(
                            [
                                ServicesMessages.DeleteOperation,
                                CategoryMessages.Category,
                                error
                            ]
                        )
                    )
                );
            }

            var result = await _categoryRepository.GetAllByUserAsync(userId);

            if (result.IsSuccess)
            {
                _logger.LogInformation("GetCategoriesByUserAsync - Categories got sucessfully");
                var categories = CategoryResponse.FromDomainEntities(result.Success!);
                var incomes = categories.Where(c => c.Type == TransactionType.Income).ToList();
                var expenses = categories.Where(c => c.Type == TransactionType.Expense).ToList();                
                return Either<MessageResponse, CategoryBreakdownResponse>.FromSuccess(new CategoryBreakdownResponse(incomes, expenses));
            }

            _logger.LogInformation("GetCategoriesByUserAsync - Error on getting categories.");
            return Either<MessageResponse, CategoryBreakdownResponse>.FromError(
                new MessageResponse(
                    result.Error!
                )
            );
        }

        public async Task<Either<MessageResponse, CategoryResponse>> GetCategoryByIdAsync(int id)
        {
            _logger.LogInformation("GetCategoryByIdAsync - Request for get category by id {id}", id);

            if (id <= 0)
            {
                var error = ValidatorsMessages.FieldRequired.WithParameters(CategoryMessages.Id);
                _logger.LogError("GetCategoryByIdAsync - Error on validate {error}", error);

                return Either<MessageResponse, CategoryResponse>.FromError(
                    new MessageResponse(
                        ServicesMessages.AnErrorOccurred.WithParameters(
                            [
                                ServicesMessages.DeleteOperation,
                                CategoryMessages.Category,
                                error
                            ]
                        )
                    )
                );
            }

            var result = await _categoryRepository.GetByIdAsync(id);

            if (result.IsSuccess)
            {
                _logger.LogInformation("GetCategoryByIdAsync - Category got sucessfully");
                return Either<MessageResponse, CategoryResponse>.FromSuccess(CategoryResponse.FromDomainEntity(result.Success!));
            }

            _logger.LogInformation("GetCategoriesByUserAsync - Error on getting categories.");
            return Either<MessageResponse, CategoryResponse>.FromError(
                new MessageResponse(
                    result.Error!
                )
            );
        }

        public async Task<Either<MessageResponse, CategoryResponse>> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            _logger.LogInformation("UpdateCategoryAsync - Request Received {request}", request);

            var validationResult = _updateCategoryRequestValidator.Validate(request);

            if (validationResult.Errors.Count > 0)
            {
                var error = validationResult.Errors.FirstOrDefault()!.ToString();
                _logger.LogError("UpdateCategoryAsync - Error on validate {error}", error);

                return Either<MessageResponse, CategoryResponse>.FromError(
                    new MessageResponse(
                        ServicesMessages.AnErrorOccurred.WithParameters(
                            [
                                ServicesMessages.CreateOperation,
                                CategoryMessages.Category,
                                error
                            ]
                        )
                    )
                );
            }

            var category = request.ToDomainEntity();
            var result = await _categoryRepository.UpdateAsync(category);

            if (result.IsSuccess)
            {
                category = result.Success;
                _logger.LogInformation("UpdateCategoryAsync - Category updated sucessfully. Category: {category}", category);
                return Either<MessageResponse, CategoryResponse>.FromSuccess(CategoryResponse.FromDomainEntity(category!));
            }

            _logger.LogInformation("UpdateCategoryAsync - Error on update category. Category: {category}. Error {error}", [category, result.Error!]);
            return Either<MessageResponse, CategoryResponse>.FromError(
                new MessageResponse(
                    result.Error!
                )
            );
        }
    }
}
