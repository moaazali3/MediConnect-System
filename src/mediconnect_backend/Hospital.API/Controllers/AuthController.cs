using Hospital.Application.DTOs.Authentication;
using Hospital.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            try
            {
                var token = await _authService.Login(model);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(Guid refreshToken)
        {
            try
            {
                var token = await _authService.RefreshToken(refreshToken);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto model)
        {
            try
            {
                await _authService.Register(model);
                return Ok("We sent you an OTP to your email. Please check your inbox and spam/junk folder.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }

        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string email, string otp)
        {
            try
            {
                await _authService.ConfirmEmail(email, otp);
                return Ok("Email verified successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { errors = ex.Message });
            }
        }
    }
}
