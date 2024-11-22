using Microsoft.AspNetCore.Mvc;
using Poupa.AI.Application.DTOs.Categories;
using Poupa.AI.Application.DTOs.Common;
using Poupa.AI.Application.Interfaces.Services;
using Poupa.AI.Domain.Enums;

namespace Poupa.AI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;
        private readonly ILogger<CategoryController> _logger = logger;

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        [ProducesResponseType(typeof(MessageResponse), 400)]
        public async Task<IActionResult> AddCategoryAsync(CreateCategoryRequest request)
        {
            _logger.LogInformation("AddCategoryAsync - Received request {request}", request);

            var result = await _categoryService.CreateCategoryAsync(request);

            if (result.IsSuccess)
            {
                var category = result.Success!;
                _logger.LogInformation("AddCategoryAsync - Successfully created category, id: {id}", category.Id);

                return Created(string.Empty, category);
            }
            else
            {
                var error = result.Error!;
                _logger.LogError("AddCategoryAsync - Failed to create category, error: {error}", error);
                return BadRequest(error);
            }
        }

        [HttpPatch]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        [ProducesResponseType(typeof(MessageResponse), 400)]
        public async Task<IActionResult> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            _logger.LogInformation("UpdateCategoryAsync - Received request {request}", request);

            var result = await _categoryService.UpdateCategoryAsync(request);

            if (result.IsSuccess)
            {
                var category = result.Success!;
                _logger.LogInformation("UpdateCategoryAsync - Successfully updated category, id: {id}", category.Id);

                return Ok(category);
            }
            else
            {
                var error = result.Error!;
                _logger.LogError("UpdateCategoryAsync - Failed to update category, error: {error}", error);
                return BadRequest(error);
            }
        }

        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MessageResponse), 200)]
        [ProducesResponseType(typeof(MessageResponse), 400)]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            _logger.LogInformation("DeleteCategoryAsync - Received request {request}", id);

            var result = await _categoryService.DeleteCategoryAsync(id);

            if (result.IsSuccess)
            {
                var message = result.Success!;
                _logger.LogInformation("DeleteCategoryAsync - Successfully deleted category");

                return Ok(message);
            }
            else
            {
                var error = result.Error!;
                _logger.LogError("DeleteCategoryAsync - Failed to delete category, error: {error}", error);
                return BadRequest(error);
            }
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        [ProducesResponseType(typeof(MessageResponse), 400)]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            _logger.LogInformation("GetCategoryByIdAsync - Received request {request}", id);

            var result = await _categoryService.GetCategoryByIdAsync(id);

            if (result.IsSuccess)
            {
                var category = result.Success!;
                _logger.LogInformation("GetCategoryByIdAsync - Successfully retrieved category, id: {id}", category.Id);

                return Ok(category);
            }
            else
            {
                var error = result.Error!;
                _logger.LogError("GetCategoryByIdAsync - Failed to retrieve category, error: {error}", error);
                return BadRequest(error);
            }
        }

        [HttpGet("byuser")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CategoryBreakdownResponse), 200)]
        [ProducesResponseType(typeof(MessageResponse), 400)]
        public async Task<IActionResult> GetCategoriesByUserAsync(int userId)
        {
            _logger.LogInformation("GetCategoriesByUserAsync - Received request {request}", userId);

            var result = await _categoryService.GetCategoriesByUserAsync(userId);

            if (result.IsSuccess)
            {
                var category = result.Success!;
                _logger.LogInformation("GetCategoriesByUserAsync - Successfully retrieved categories");

                return Ok(category);
            }
            else
            {
                var error = result.Error!;
                _logger.LogError("GetCategoriesByUserAsync - Failed to retrieve categories, error: {error}", error);
                return BadRequest(error);
            }
        }

        [HttpGet("byuserandtype")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IReadOnlyCollection<CategoryResponse>), 200)]
        [ProducesResponseType(typeof(MessageResponse), 400)]
        public async Task<IActionResult> GetCategoriesByUserAndTypeAsync(int userId, TransactionType type)
        {
            _logger.LogInformation("GetCategoriesByUserAndTypeAsync - Received request: userId = {userId}, type = {type}", userId, type);

            var result = await _categoryService.GetCategoriesByUserAndTypeAsync(userId, type);

            if (result.IsSuccess)
            {
                var category = result.Success!;
                _logger.LogInformation("GetCategoriesByUserAndTypeAsync - Successfully retrieved categories");

                return Ok(category);
            }
            else
            {
                var error = result.Error!;
                _logger.LogError("GetCategoriesByUserAndTypeAsync - Failed to retrieve categories, error: {error}", error);
                return BadRequest(error);
            }
        }
    }
}
