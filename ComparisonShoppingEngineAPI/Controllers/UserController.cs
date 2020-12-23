using System.Threading.Tasks;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Data.Services.UserService;
using ComparisonShoppingEngineAPI.DTOs.Image;
using ComparisonShoppingEngineAPI.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ComparisonShoppingEngineAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("user")]

    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdate)
        {
            ServiceResponse<GetUserUpdate> serviceResponse = await _userService.Update(userUpdate);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            return Ok(serviceResponse);
        }
        [HttpGet]
        public async Task<IActionResult> GetProfileImage()
        {
            ServiceResponse<GetImageDto> serviceResponse = await _userService.GetProfileImage();

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            return Ok(serviceResponse);
        }
    }
}
