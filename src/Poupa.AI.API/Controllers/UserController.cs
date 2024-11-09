using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
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
                _logger.LogInformation("AddUser - Created user failed, error: {error}", error);

                return BadRequest(error);
            }
        }
    }
}
