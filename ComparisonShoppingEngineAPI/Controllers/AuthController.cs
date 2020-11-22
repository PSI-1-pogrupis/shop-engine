using ComparisonShoppingEngineAPI.Data;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            ServiceResponse<int> response = await _authRepository.Register(
                new User { UserEmail = request.UserEmail, UserName = request.UserName }, request.Password
            );
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            ServiceResponse<string> response = await _authRepository.Login(
                request.Username, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
