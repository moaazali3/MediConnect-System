using Hospital.Application.DTOs.Authentication;

namespace Hospital.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> Login(UserLoginDto model);
        Task Register(UserRegisterDto model);
        Task<AuthDto> RefreshToken(Guid refreshToken);
        Task ConfirmEmail(string email, string otp);
    }
}
