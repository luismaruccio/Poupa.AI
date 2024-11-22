using Microsoft.AspNetCore.Mvc;
using Poupa.AI.Application.DTOs.Common;
using Poupa.AI.Application.DTOs.Users;
using Poupa.AI.Application.Interfaces.Services;

namespace Poupa.AI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, ILogger<UserController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ILogger<UserController> _logger = logger;

        /// <summary>
        /// Creates a User.
        /// </summary>
        /// <param name="request">The request containing user information to be created.</param>
        /// <returns>The newly created user.</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /User
        ///     {
        ///         "name": "John Doe",
        ///         "email": "john.doe@example.com",
        ///         "password": "P@ssw0rd"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Returns the newly created user</response>
        /// <response code="400">If the request is invalid</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateUserResponse), 200)]
        [ProducesResponseType(typeof(MessageResponse), 400)]
        public async Task<IActionResult> AddUserAsync(CreateUserRequest request)
        {
            _logger.LogInformation("AddUser - Received request {request}", request);

            var result = await _userService.CreateUserAsync(request);

            if (result.IsSuccess)
            {
                var user = result.Success!;
                _logger.LogInformation("AddUser - Created user with success, id: {id}", user.Id);

                return Ok(user);
            }
            else
            {
                var error = result.Error!;
                _logger.LogError("AddUser - Created user failed, error: {error}", error);

                return BadRequest(error);
            }
        }
    }
}
